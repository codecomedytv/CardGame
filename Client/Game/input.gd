extends Control
class_name GameInput

signal COMMAND_REQUESTED
var network_id: int
var player: Object
var opponent: Object
var messenger: Object
var cards: Dictionary

func setup(_player: Object, _opponent: Object, _identifier: int, commands: Object, _cards: Dictionary) -> void:
	player = _player
	opponent = _opponent
	network_id = _identifier
	messenger = commands
	cards = _cards

func _deploy(card: Object) -> void:
	if player.state != player.IDLE or card.card_type != CARD_TYPE.UNIT or not card in player.hand:
		return
	messenger._deploy(card.id)

func _set_facedown(card: Object) -> void:
	if player.state != player.IDLE or card.card_type != CARD_TYPE.SUPPORT or not card in player.hand:
		return
	messenger._set_face_down(card.id)
	
func _activate(card: Object, skill: int = 0, targets: Array = []) -> void:
#	if not player.active or card.card_type != CARD_TYPE.SUPPORT  or not card.ready or not card.legal or not card in player.support:
#		return
	messenger._activate(card.id, skill, targets)
	
func _target(card: Object) -> void:
	messenger._target(card.id)

func _declare_attack(attacker: Object, defender) -> void:
	if player.state != player.IDLE:
		return
	if not attacker in player.field or not attacker.ready:
		return
	if defender is Object:
		defender.remove_aura()
		defender = defender.id
	messenger._declare_attack(attacker.id, defender)
	
func _pass_priority() -> void:
	messenger._pass_priority()

func _end_turn() -> void:
	messenger._end_turn()

func _command() -> void:
	emit_signal("COMMAND_REQUESTED")
