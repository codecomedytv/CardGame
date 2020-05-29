extends Tween

enum {
	GLOBAL
	PLAYER
	OPPONENT
}

var opponent_delay: float = 0
var player_delay: float = 0

# This probably shouldn't be here BUT its currently the only thing that uses
# meaningfully

func _ready() -> void:
	connect("tween_all_completed", self, "_reset")

func add_delay(delay: float, who: int) -> float:
	if who == PLAYER:
		player_delay += delay
	elif who == OPPONENT:
		opponent_delay += delay
	return total_delay(who)
	
func total_delay(who: int) -> float:
	return player_delay if who == PLAYER else opponent_delay
	
func _reset() -> void:
	opponent_delay = 0
	player_delay = 0
	remove_all()
