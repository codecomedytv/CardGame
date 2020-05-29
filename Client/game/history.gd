extends RichTextLabel

enum {
	GLOBAL
	PLAYER
	OPPONENT
}

var line: int = 0


func pdraw(drawn: Array) -> void:
	bbcode_text += "%s : You drew %s\n" % [line, drawn]
	line += 1

func odraw(count: int) -> void:
	bbcode_text += "%s : Enemy drew %s card(s)\n" % [line, count]
	line += 1
	
func deploy(player: int, card: Object) -> void:
	var p = "You" if player == PLAYER else "Enemy"
	bbcode_text += "%s : %s deployed %s\n" % [line, p, card]
	line += 1
	
func set_facedown(player: int, card: Object) -> void:
	if player == PLAYER:
		bbcode_text += "%s : You set %s\n" % [line, card]
	else:
		bbcode_text += "%s : Enemy set a card facedown\n" % line
	line += 1
	
func target(player: int, targets: Array) -> void:
	if player == PLAYER:
		bbcode_text += "%s : \tTargeting: %s\n" % [line, targets]
	else:
		bbcode_text += "%s : \tTargeting: %s\n" % [line, targets]
	line += 1
	
func bounce(player: int, card: Object) -> void:
	if player == PLAYER:
		bbcode_text += "%s : %s was returned to your hand\n" % [line, card]
	else:
		bbcode_text += "%s : %s was returned to Enemy's hand\n" % [line, card]
	line += 1
	
func attack(player: int, attacker: Object, defender: Object) -> void:
	if player == PLAYER:
		bbcode_text += "%s : Your %s attacked:\n\t  Enemy's %s\n" % \
		[line, attacker, defender]
	else:
		bbcode_text += "%s : Enemy's %s attacked:\n\t  Your %s\n" % \
		[line, attacker, defender]
	line += 1
	
func activate(player: int, card: Object) -> void:
	if player == PLAYER:
		bbcode_text += "%s : You activated %s\n" % [line, card]
	else:
		bbcode_text += "%s : Enemy activated %s\n" % [line, card]
	line += 1
	
func direct_attack(player: int, attacker: Object) -> void:
	if player == PLAYER:
		bbcode_text += "%s : Your %s attacked directly\n" % [line, attacker]
	else:
		bbcode_text += "%s : Enemy's %s attacked directly\n" % [line, attacker]
	line += 1
	
func destroy_unit(who: int, unit: Object) -> void:
	if who == PLAYER:
		bbcode_text += "%s : Your %s was destroyed\n" % [line, unit]
	else:
		bbcode_text += "%s : Enemy's %s was destroyed\n" % [line, unit]
	line += 1
	
func lose_life(who: int, amount: int) -> void:
	if who == PLAYER:
		bbcode_text += "%s : You took %s damage\n" % [line, amount]
	else:
		bbcode_text += "%s : Enemy took %s damage\n" % [line, amount]
	line += 1
	
func begin_turn() -> void:
	if line > 2: # 0 and 1 are taken by starting hands 
		bbcode_text += "%s : Enemy's turn has ended\n" % line
		line += 1
	bbcode_text += "%s : Your turn has started\n" % line
	line += 1
	
func end_turn() -> void:
	bbcode_text += "%s : Your turn has ended\n" % line
	line += 1
	bbcode_text += "%s : Enemy's turn has started\n" % line
	line += 1

