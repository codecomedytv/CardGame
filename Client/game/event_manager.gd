extends Reference

var _events: Array = []
signal COMMAND_REQUESTED
signal BATTLE
signal ANIMATED
var player: Object
var opponent: Object
var input

func setup(_player: Object, _opponent: Object, _input) -> void:
	player = _player
	opponent = _opponent
	input = _input

func _queue(event: int, arguments: Array) -> void:
	_events.append({"command": event, "args": arguments})

func _execute() -> void:
	for event in _events:
		match event.command:
			EVENT.DRAW:
				player.draw(event.args)
			EVENT.LOAD_DECK:
				player.load_deck(event.args)
			EVENT.OPPONENT_DRAW:
				opponent.draw(event.args)
			EVENT.OPPONENT_LOAD_DECK:
				opponent.load_deck(event.args)
			EVENT.BEGIN_TURN:
				player.begin_turn()
			EVENT.END_TURN:
				player.end_turn()
			EVENT.WIN:
				player.win()
			EVENT.LOSE:
				player.lose()
			EVENT.DEPLOY:
				player.deploy(event.args)
			EVENT.OPPONENT_DEPLOY:
				opponent.deploy(event.args)
			EVENT.SET_FACEDOWN:
				player.set_facedown(event.args)
			EVENT.OPPONENT_SET_FACEDOWN:
				opponent.set_facedown(event.args)
			EVENT.LOSE_LIFE:
				player.lose_life(event.args)
			EVENT.OPPONENT_LOSE_LIFE:
				opponent.lose_life(event.args)
			EVENT.CARD_DESTROYED:
				player.destroy_unit(event.args)
			EVENT.OPPONENT_CARD_DESTROYED:
				opponent.destroy_unit(event.args)
			EVENT.ATTACKED_UNIT:
				player.attack_unit(event.args)
			EVENT.OPPONENT_ATTACKED_UNIT:
				opponent.attack_unit(event.args)
			EVENT.READY_CARD:
				player.ready_cards(event.args)
			EVENT.UNREADY_CARD:
				player.unready_cards(event.args)
			EVENT.LEGALIZE:
				player.legalize(event.args)
			EVENT.FORBID:
				player.forbid(event.args)
			EVENT.ATTACKED_DIRECTLY:
				player.attack_directly(event.args)
			EVENT.OPPONENT_ATTACKED_DIRECTLY:
				opponent.attack_directly(event.args)
			EVENT.OPPONENT_ACTIVATE:
				opponent.activate(event.args)
			EVENT.RESOLVE:
				player.resolve()
				opponent.resolve()
				player.link.clear()
			EVENT.SET_TARGETS:
				print("setting targets (client) -> ", event)
				player.set_targets(event.args)
			EVENT.BOUNCE:
				player.bounce(event.args)
			EVENT.OPPONENT_BOUNCE:
				opponent.bounce(event.args)
			EVENT.ATTACK_DECLARED:
				opponent.show_attack(event.args)
			EVENT.AUTO_TARGET:
				player.autotarget(event.args)
				print("auto_target")
				print(event)
			EVENT.SET_STATE:
				player.set_state(event.args)
			EVENT.SET_DEPLOYABLE:
				player.set_deployable(event.args)
			EVENT.NO_OP:
				pass
			EVENT.SET_SETTABLE:
				player.set_settable(event.args)
			EVENT.SET_ACTIVATABLE:
				player.set_activatable(event.args)
			_:
				push_warning("Should have been unreachable")
				print_debug(event)
	emit_signal("ANIMATED")
	_events.clear()
	
func _on_animation_finished() -> void:
	emit_signal("COMMAND_REQUESTED")
