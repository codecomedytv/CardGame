extends GameInput

var attacked = false
var deployed = false

func _command():
	if _game_over() or not _is_our_turn():
		return
	randomize()
	if not deployed:
		deploy()
		deployed = true
	elif not attacked:
		attack()
		attacked = true
	else:
		end()

func attack():
	if player.field.size() > 0 and opponent.field.size() > 0:
		var attacker = player.field[randi() % player.field.size()]
		var defender = opponent.field[randi() % opponent.field.size()]
		if not attacker.ready:
			call_deferred("_command")
			return
		_declare_attack(attacker.id, defender.id)
		return
	else:
		end()
		
func deploy():
	if player.hand.size() > 0 and player.field.size() < 7:
		var card = player.hand[randi() % player.hand.size()]
		if card.card_type == CARD_TYPE.SUPPORT:
			push_warning("Attempting to deploy support card: %s" % card.title)
			end()
			return
		if card.card_type == CARD_TYPE.UNIT:
			_deploy(card)
			return
	else:
		end()
		
func end():
	deployed = false
	attacked = false
	_end_turn()
	
func _game_over() -> bool:
	return player.lost or player.won
	
func _is_our_turn() -> bool:
	return player.is_turn_player
