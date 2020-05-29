extends Reference

enum {
	IDLE # An IDLE Player can take any primary (non-link) action
	ACTIVE # An Active Player can take any secondary (link) action
	PASSIVE # A Passive Player can take no action (link or non-link)
	ACTING # An Acting Player has taken an action in a link
	PASSING # A Passing Player has took no action in a link when prompted
	TARGETING # A Targeting Player is targeting a card
}

var state: int = PASSIVE
var is_turn_player: bool = false
var life: int = 8000
var decksize: int = 0
var hand: Array = []
var field: Array = []
var discard: Array = []
var support: Array = []
var won: bool = false
var lost: bool = false
signal WON
signal LOST
var visual: Control
var cards: Dictionary = {}
var opponent: Object
var active: bool = false setget ,_get_active
var link = []
var input
#var state: String

func _get_active() -> bool:
	return state == IDLE or state == ACTIVE

func set_state(args: Array) -> void:
	state = args[0]
	visual.set_state(state)
	
func set_deployable(args: Array) -> void:
	cards[args[0]].can_be_deployed = true
	
func set_settable(args: Array) -> void:
	cards[args[0]].can_be_set = true
	
func set_activatable(args: Array) -> void:
	cards[args[0]].can_be_activated = true

func autotarget(args: Array) -> void:
	print(args)
	var targeter = cards[args[0]]
	# change states
#	state = "Targeting"
	state = TARGETING
	visual.autotarget(targeter)
#	_show_valid_targets(targeter)


func setup(_cards: Dictionary = {}) -> void:
	cards = _cards
	
func set_targets(args: Array) -> void:
	cards[args[0]].valid_targets = args[1]
	
func resolve() -> void:
	var linked: Array = []
	for card in link:
		card.activated = false
		if card.under_players_control:
			linked.append(card)
	visual.resolve(linked)

func attack_unit(args: Array) -> void:
	var attacker: Object = cards[args[0]]
	var defender: Object = cards[args[1]]
	visual.attack_unit(attacker, defender)
	
func attack_directly(args: Array) -> void:
	var attacker: Object = cards[args[0]]
	visual.attack_directly(attacker)
	
func bounce(args: Array) -> void:
	var card: Object = cards[args[0]]
	field.erase(card)
	hand.append(card)
	card.zone = card.Zone.HAND
	visual.bounce(card)
	
func deploy(args: Array) -> void:
	var card: Object = cards[args[0]]
	hand.erase(card)
	field.append(card)
	card.zone = card.Zone.UNIT
	visual.deploy(args)
	
func activate(args: Array) -> void:
	var card: Object = cards[args[0]]
	support.erase(card)
	discard.append(card)
	card.zone = card.Zone.DISCARD
	visual.activate(card, link, [])

func set_facedown(args: Array) -> void:
	var card: Object = cards[args[0]]
	hand.erase(card)
	support.append(card)
	card.zone = card.Zone.SUPPORT
	visual.set_facedown(args)
	
func forbid(args: Array) -> void:
	for id in args:
		var card = cards[id]
		card.legal = false
		
var interact: AnimatedSprite
func draw(args: Array) -> void:
	decksize -= args.size()
	for c in args:
		var card: Object = ClientLibrary.fetch(c)
		card.connect("activated", input, "_activate")
		card.player = self
		card.interact = interact
		card.under_players_control = true
		cards[card.id] = card
		hand.append(card)
		card.zone = card.Zone.HAND
	visual.draw(args, self)

func destroy_unit(args: Array) -> void:
	var card = cards[args[0]]
	field.erase(card)
	discard.append(card)
	card.zone = card.Zone.DISCARD
	visual.destroy_unit(args)
	
func lose_life(args: Array) -> void:
	life -= args[0]
	visual.lose_life(args)
	
func legalize(card_ids: Array) -> void:
	for id in card_ids:
		cards[id].legal = true
	
func ready_cards(card_ids: Array) -> void:
	for id in card_ids:
		cards[id].ready = true
	visual.ready_cards(card_ids)

func unready_cards(card_ids: Array) -> void:
	for id in card_ids:
		cards[id].ready = false
	visual.unready_cards(card_ids)

func load_deck(args: Array) -> void:
	var _decksize: int = args[0]
	decksize = _decksize
	visual.load_deck(args)

func begin_turn() -> void:
	is_turn_player = true
	visual.begin_turn()

func end_turn() -> void:
	is_turn_player = false
	visual.end_turn()

func win() -> void:
	won = true
	emit_signal("WON")

func lose() -> void:
	lost = true
	emit_signal("LOST")
