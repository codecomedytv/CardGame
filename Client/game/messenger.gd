extends Node

signal RETRY
signal _queue_event
signal _execute_events
signal _disconnect
const SERVER: int = 1
var id: int = 0

func _init() -> void:
	name = "messenger"

puppet func _queue_event(command: int, arguments: Array) -> void:
	emit_signal("_queue_event", command, arguments)

puppet func _execute_events() -> void:
	emit_signal("_execute_events")

puppet func _retry() -> void:
	emit_signal("RETRY")
	
puppet func _force_disconnected(reason: int) -> void:
	emit_signal("_disconnect", reason)

func _deploy(card_id: int) -> void:
	rpc_id(SERVER, "_deploy", id, card_id)
	
func _declare_attack(attacker_id: int, target_id: int) -> void:
	rpc_id(SERVER, "_attack", id, attacker_id, target_id)
	
func _activate(card_id: int, skill: int = 0, targets: Array = []) -> void:
	rpc_id(SERVER, "_activate", id, card_id, skill, targets)
	
func _target(target_id: int) -> void:
	rpc_id(SERVER, "_target", id, target_id)
	
func _set_face_down(card_id: int) -> void:
	rpc_id(SERVER, "_set_facedown", id, card_id)
	
func _pass_priority() -> void:
	rpc_id(SERVER, "_pass_priority", id)

func _end_turn() -> void:
	rpc_id(SERVER, "_end_turn", id)

func set_ready() -> void:
	rpc_id(SERVER, "_set_ready", id)
