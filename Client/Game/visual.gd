extends Control

# Visual Player/Opponent
onready var life: Control = $View/Life
onready var hand: Control = $Hand
onready var units: Control = $Units
onready var support: Control = $Support
onready var discard: Control = $Discard
onready var deck: Control = $Deck
onready var damage: Control = $Damage
var animate: Tween
var who: int
var SFX: AudioStreamPlayer
var history: Control
var cards: Dictionary

func delay(_delay = null) -> float:
	if _delay is float:
		return animate.add_delay(_delay, who)
	else:
		return animate.total_delay(who)

func autotarget(card: Object) -> void:
	for target in card.valid_targets:
		_queue_callback(cards[target], delay(), "show_as_valid", true)
	
func _queue_property(object: Object, property: String, start, end, duration: float, delay: float = 0.0) -> void:
	animate.interpolate_property(object, property, start, end, duration, Tween.TRANS_LINEAR, Tween.EASE_IN, delay)
	
func _queue_callback(object: Object, delay: float, callback: String, arg1 = null, arg2 = null, arg3 = null) -> void:
	animate.interpolate_callback(object, delay, callback, arg1, arg2)
		
signal button_action
func set_state(state: int) -> void:
	var x = {0: "IDLE", 1: "ACTIVE", 2: "PASSIVE", 3: "ACTING", 4: "PASSING",
	5: "TARGETING"}
	if state == 0:
		emit_signal("button_action", "")
	elif state == 1:
		emit_signal("button_action", "Pass")
	else:
		emit_signal("button_action", "Wait")
	var _state = x[state]
	$Active.text = _state

func show_attack(attacker: Object, defender) -> void:
	attacker.Combat.show()
	if defender is Object:
		defender.Combat.show()
		history.attack(who, attacker, defender)
	else:
		history.direct_attack(who, attacker)

func setup(_animate: Tween, sfx: AudioStreamPlayer, _history: Control, _who: int) -> void:
	animate = _animate
	SFX = sfx
	who = _who
	history = _history
	
func bounce(card: Object) -> void:
	_queue_property(card, "rect_global_position", card.rect_global_position, future_position(hand), 0.3, delay())
	_queue_callback(history, delay() + 0.3, "bounce", who, card)
	_queue_callback(card.get_parent(), delay(0.3), "remove_child", card)
	_queue_callback(hand, delay(), "add_child", card)
	_queue_callback(SFX, delay(), "_deploy")
	if who == animate.OPPONENT:
		_queue_callback(card, delay(), "flip_face_down")
		var fake = ClientLibrary.placeholder()
		_queue_callback(hand, delay(), "remove_child", card)
		_queue_callback(hand, delay(), "add_child", fake)
		_queue_callback(card, delay(), "queue_free")
	
	
func resolve(linked: Array) -> void:
	for card in linked:
		_queue_callback(card.get_parent(), delay(0.3), "remove_child", card)
		_queue_callback(discard, delay(), "add_child", card)
		# Should add a check for unit based effects
		_queue_property(card, "rect_global_position", card.rect_global_position, discard.rect_global_position, 0.3, delay())

func activate(card: Object, link, targets: Array) -> void:
	if who == animate.OPPONENT:
		# Track a position element in future so we can replicate same layout
		# as server/client
		support.get_children().front().free()
		support.add_child(card)
		card.Back.visible = true
	# Add a link dependecy
	link.append(card)
	_queue_callback(card.Link, delay(), "set_text", link.size() as String)
	_queue_callback(card.Link, delay(0.1), "set_visible", true)
	_queue_callback(card, delay(), "flip_face_up")
	_queue_callback(SFX, delay(), "_deploy")
	_queue_callback(card.Back, delay(0.1), "hide")
	_queue_callback(history, delay(0.1), "activate", who, card)
	if not targets.empty():
		_queue_callback(history, delay(0.1), "target", who, targets)

func attack_unit(attacker: Object, defender: Object) -> void:
	var target_position: Vector2 = _attack_target_position(defender, who)
	_queue_property(attacker, "rect_global_position", attacker.rect_global_position, target_position, .1, delay())
	_queue_property(attacker, "rect_global_position", target_position, attacker.rect_global_position, .1, delay(0.1))
	_queue_callback(attacker.Combat, delay(), "hide")
	_queue_callback(defender.Combat, delay(), "hide")
	_queue_callback(defender, delay(), "remove_aura")
	if who == animate.PLAYER:
		_queue_callback(history, delay(), "attack", who, attacker, defender)
	_queue_callback(SFX, delay(.3), "_battle_unit")

func _attack_target_position(defender: Object, player: int) -> Vector2:
	var y_modifier: Vector2 = Vector2(0, defender.rect_size.y)
	return defender.rect_global_position + y_modifier if player == animate.PLAYER else defender.rect_global_position - y_modifier
	
func attack_directly(attacker: Object) -> void:
	var target_position = _direct_attack_target_position(attacker, who)
	_queue_property(attacker, "rect_global_position", attacker.rect_global_position, target_position, .3, delay())
	_queue_property(attacker, "rect_global_position", target_position, attacker.rect_global_position, .3, delay(0.3))
	if who == animate.PLAYER:
		animate.delay(0.3, animate.OPPONENT)
		# Prevent lost-life showing up too quickly!
	_queue_callback(attacker.Combat, delay(), "hide")
	_queue_callback(history, delay(0.1), "direct_attack", who, attacker)
	_queue_callback(SFX, delay(.3), "_battle_unit")
	
func _direct_attack_target_position(attacker, player: int) -> int:
	var y_modifier: Vector2 = Vector2(0, 70)
	return attacker.rect_global_position - y_modifier if player == animate.PLAYER else attacker.rect_global_position + y_modifier
	
func ready_cards(args: Array) -> void:
	for id in args:
		_queue_callback(cards[id], delay(), "ready")
	
func unready_cards(args: Array) -> void:
	for id in args:
		_queue_callback(cards[id], delay(), "exhaust")
	
func deploy(args: Array) -> void:
	var card: Object = cards[args[0]] if who == animate.PLAYER else cards[args[0].id]
	if who == animate.OPPONENT:
		hand.remove_child(hand.get_child(0))
		hand.add_child(card)
		sort(hand)
		card.flip_face_down()
	_queue_property(card, "rect_global_position", card.rect_global_position, future_position(units), 0.3, delay())
	_queue_callback(history, delay() + 0.3, "deploy", who, card)
	_queue_callback(card.get_parent(), delay(0.3), "remove_child", card)
	_queue_callback(units, delay(), "add_child", card)
	_queue_callback(card, delay(), "flip_face_up")
	_queue_callback(SFX, delay(), "_deploy")

func set_facedown(args: Array = []) -> void:
	var card: Object = cards[args[0]] if who == animate.PLAYER else ClientLibrary.placeholder()
	if who == animate.OPPONENT:
		hand.remove_child(hand.get_child(0))
		hand.add_child(card)
		sort(hand)
	_queue_property(card, "rect_global_position", card.rect_global_position, future_position(support), 0.3, delay())
	_queue_callback(history, delay() + 0.3, "set_facedown", who, card)
	_queue_callback(card.get_parent(), delay(0.3), "remove_child", card)
	_queue_callback(support, delay(), "add_child", card)
	_queue_callback(card, delay(), "flip_face_down")
	_queue_callback(SFX, delay(), "_set_support")
	
func lose_life(args: Array) -> void:
	var damage_taken: int = args[0]
	damage.text = "-%s" % args[0] as String
	life.text = (life.text as int - damage_taken) as String
	var visible: Color = damage.modulate + Color(0, 0, 0, 255)
	var invisible: Color = damage.modulate - Color(0, 0, 0, 255)
	_queue_callback(damage, delay(), "set_self_modulate", visible)
	_queue_callback(history, delay(0.1), "lose_life", who, damage_taken)
	_queue_callback(damage, delay(0.5), "set_self_modulate", invisible)

func destroy_unit(args: Array) -> void:
	var card: Object = cards[args[0]]
	_queue_callback(card.get_parent(), delay(0.3), "remove_child", card)
	_queue_callback(discard, delay(), "add_child", card)
	_queue_property(card, "rect_global_position", card.rect_global_position, discard.rect_global_position, 0.3, delay())
	_queue_callback(history, delay() + 0.1, "destroy_unit", who, card)
	
func load_deck(args: Array) -> void:
	var decksize: String = args[0] as String
	_queue_callback(deck, delay(0.3), "set_text", decksize)

func draw(args: Array, player_data: Object) -> void:
	var count: int = 0
	count = args.size() if who == animate.PLAYER else args[0]
	var drawn: Array = []
	var positions = next_hand_positions(count)
	for i in count:
		var card: Object = cards[args[i].id] if who == animate.PLAYER else ClientLibrary.placeholder()
		drawn.append(card)
		hand.add_child(card)
		card.rect_global_position = deck.rect_global_position
		card.turn_invisible()
		_queue_property(card, "rect_global_position", card.rect_global_position, positions.pop_front(), 0.2, delay(0.2))
		_queue_callback(card, delay(0.0), "turn_visible")
		var decksize: String = (player_data.decksize + count - i - 1) as String
		_queue_callback(deck, delay(), "set_text", decksize)
		_queue_callback(SFX, delay(), "_draw_card")
	_queue_callback(self, delay(0.2), "sort", hand)
	if who == animate.PLAYER:
		_queue_callback(history, delay(), "pdraw", drawn)
	else:
		_queue_callback(history, delay(), "odraw", count)

func next_hand_positions(count: int) -> Array:
	var blanks: Array = []
	var positions: Array = []
	for copy in count:
		var blank: Object = ClientLibrary.placeholder()
		hand.add_child(blank)
		blanks.append(blank)
	sort(hand)
	for blank in blanks:
		positions.append(blank.rect_global_position)
	for blank in blanks:
		hand.remove_child(blank)
		blank.free()
	blanks.clear()
	return positions
	
func future_position(zone: Control) -> Vector2:
	var blank: Object = ClientLibrary.placeholder()
	zone.add_child(blank)
	sort(zone)
	var next_position: Vector2 = blank.rect_global_position
	zone.remove_child(blank)
	return next_position
	
func sort(zone: Container) -> void:
	zone.notification(Container.NOTIFICATION_SORT_CHILDREN)

func begin_turn() -> void:
	_queue_callback(history, delay(), "begin_turn")

func end_turn() -> void:
	_queue_callback(history, delay(), "end_turn")
