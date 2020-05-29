extends GameInput

var interact: AnimatedSprite
var _selecting_target: bool = false
var _activated: Object

func _process(delta: float) -> void:
	if Input.is_action_just_pressed("drag_drop"):
		var mouse: Vector2 = get_global_mouse_position()
		if interact.visible:
			# Cards place themselves here first
			_drop(mouse)

func deploy(card: Object) -> void:
	call_deferred("_deploy", card)
	
func set_facedown(card: Object) -> void:
	card.legal = false
	call_deferred("_set_facedown", card)
	
func declare_attack(attacker: Object, defender: Object) -> void:
	call_deferred("_declare_attack", attacker, defender.id)
	
func direct_attack(attacker: Object) -> void:
	call_deferred("_declare_attack", attacker, -1)
	
func pass_priority() -> void:
	_pass_priority() 
	
func end_turn() -> void:
	_end_turn()
	
#elif _activating(mouse):
##		for card in player.support:
##			if card.get_global_rect().has_point(mouse) and card.legal and card.ready and card.card_type == CARD_TYPE.SUPPORT:
##				activate(card, 0, card.selected_targets)
	
#func activate(card: Object, skill_index: int = 0, targets: Array = []) -> void:
##	# If we have selected no targets while we have targets to select and we're not currently selecting a target
##	# then lets animate and select a target
##	if card.activated:
##		return
##	if targets.empty() and not card.valid_targets.empty():
##		_selecting_target = true
##		player.activate([card.id])
##		player.visual.animate.start()
##		_show_valid_targets(card)
##		return
##	elif card.does_not_require_targets():
##		player.activate([card.id])
##		player.visual.animate.start()
##	card.activated = true
##	card.legal = false
#	call_deferred("_activate", card, skill_index, targets)
#
#	_stop_targeting(card)

func _show_valid_targets(card) -> void:
	_activated = card
	_selecting_target = true # state here
	for valid_target in card.valid_targets:
		cards[valid_target].show_as_valid(true)

func _stop_targeting(card) -> void:
	for valid_target in card.valid_targets:
		cards[valid_target].show_as_valid(false)
		card.selected_targets = []
		card.valid_targets = []
		_selecting_target = false
		_activated = null

#func _select_target(mouse: Vector2) -> void:
#	for id in _activated.valid_targets:
#		if cards[id].get_global_rect().has_point(mouse):
#			_activated.selected_targets.append(id)
#			cards[id].show_as_targeted() 
#			player.visual.history.target(player.visual.animate.PLAYER, [cards[id]])
#			player.visual.animate.start()
#			activate(_activated, 0, _activated.selected_targets)

#func _drag(mouse: Vector2) -> void:
#	if not player.active:
#		return
#	if player.state == player.TARGETING:
#		for card in cards.values():
#			if card.get_global_rect().has_point(mouse) and card.ValidTarget.playing:
#				for c in cards.values():
#					c.remove_aura()	
#				player.active = false
#				player.state = ""
#				_target(card)
#				return
#	elif _selecting_target:
#		_select_target(mouse)
#	elif _activating(mouse):
#		for card in player.support:
#			if card.get_global_rect().has_point(mouse) and card.legal and card.ready and card.card_type == CARD_TYPE.SUPPORT:
#				activate(card, 0, card.selected_targets)
#	elif _playing(mouse):
#		for card in player.hand:
#			if card.get_global_rect().has_point(mouse):
#				interact.add(card)
#	elif _battling(mouse):
#		for card in player.field:
#			if card.get_global_rect().has_point(mouse) and card in player.field and card.ready and not card.valid_targets.empty():
#				interact.add(card)
#				_show_valid_targets(card)

func _battling(mouse: Vector2) -> bool:
	return player.visual.units.get_global_rect().has_point(mouse)
			
func _drop(mouse: Vector2) -> void:
	if _deploying(mouse):
		deploy(interact.card)
	if _setting_facedown(mouse):
		set_facedown(interact.card)
	if _declaring_attack(mouse):
		_battle(mouse)
	interact.stop()
	
func _battle(mouse) -> void:
	if not interact.card in player.field:
		return
	var attacker: Object = interact.card
	var defender: Object
	
	for card in cards.values():
		if card.get_global_rect().has_point(mouse):
			if card in opponent.field:
				defender = card
				break
				
	if not defender.id in attacker.valid_targets:
		_stop_targeting(attacker)
		return
				
	if attacker != null and defender != null:
		attacker.Combat.show()
		defender.Combat.show()
		declare_attack(attacker, defender)

	if defender == null and opponent.field.empty():
		attacker.Combat.show()
		direct_attack(attacker)
		

func _deploying(mouse: Vector2) -> bool:
	return player.visual.units.get_global_rect().has_point(mouse) and interact.card.can_be_deployed
	
func _setting_facedown(mouse: Vector2) -> bool:
	return player.visual.support.get_global_rect().has_point(mouse) and interact.card.can_be_set

func _declaring_attack(mouse: Vector2) -> bool:
	return opponent.visual.units.get_global_rect().has_point(mouse)
	
func _activating(mouse: Vector2) -> bool:
	return player.visual.support.get_global_rect().has_point(mouse)
	
func _playing(mouse: Vector2) -> bool:
	return player.visual.hand.get_global_rect().has_point(mouse)
