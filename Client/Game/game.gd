extends Control

onready var WinLoseNotification = $WIN_LOSE
onready var GFX: Tween = $Effects/GFX_Visual
onready var SFX: AudioStreamPlayer = $Effects/SFX_Audio
onready var BGM: AudioStreamPlayer = $Effects/BGM_Audio
onready var History: RichTextLabel = $History
var messenger: Node = preload("messenger.gd").new()
var player: Reference = preload("player.gd").new()
var opponent: Reference = preload("opponent.gd").new()
var event_manager: Reference = preload("event_manager.gd").new()
var input: GameInput
var muted: bool = false
var cards: Dictionary = {}
var link: Array = []

func setup(mute: bool, _input: Object) -> void:
	input = _input
	muted = mute
	GFX.history = History
	add_child(messenger, true)
	player.visual = $Player
	opponent.visual = $Opponent
	$Player.setup(GFX, SFX, History, GFX.PLAYER)
	$Opponent.setup(GFX, SFX, History, GFX.OPPONENT)
	
	var network_id: int = multiplayer.get_network_unique_id()
	messenger.id = network_id
	SFX.stream_paused = muted
	BGM.stream_paused = muted
	
	player.setup(cards)
	opponent.setup(cards)
	player.link = link
	opponent.link = link
	player.opponent = opponent
	opponent.opponent = player
	player.visual.cards = cards
	opponent.visual.cards = cards
	event_manager.setup(player, opponent, input)
	input.setup(player, opponent, network_id, messenger, cards)
	if "interact" in input:
		input.interact = $Interact
		player.interact = $Interact
		player.input = input
#		for card in cards:
#			cards[card].interact = $Interact
	add_child(input)
	$Background/EndTurn.connect("pressed", input, "_end_turn")
	$Background/Pass.connect("pressed", input, "_pass_priority")
	player.visual.connect("button_action", $Background/Pass, "set_text")
	_connect(event_manager, "COMMAND_REQUESTED", input, "_command")
	_connect(event_manager, "ANIMATED", GFX, "start")

	_connect(player, "WON", self, "_win")
	_connect(player, "LOST", self, "_lose")
	
	_connect(messenger, "_queue_event", event_manager, "_queue")
	_connect(messenger,"_execute_events", event_manager, "_execute")
	_connect(messenger, "_disconnect", get_parent(), "_force_disconnect")
	
	_connect(GFX, "tween_all_completed", event_manager, "_on_animation_finished")
	
	messenger.call_deferred("set_ready")



func _win() -> void:
	GFX.InterpolateCallback(BGM, GFX.TotalDelay(GFX.PLAYER), "stop")
	GFX.InterpolateCallback(SFX, GFX.add_delay(0.2, GFX.PLAYER), "_victory")
	GFX.InterpolateCallback(WinLoseNotification, GFX.TotalDelay(GFX.PLAYER), "set_text", "YOU WIN!")
	GFX.InterpolateCallback(WinLoseNotification, GFX.TotalDelay(GFX.PLAYER), "set_visible", true)

func _lose() -> void:
	GFX.InterpolateCallback(BGM, GFX.TotalDelay(GFX.PLAYER), "stop")
	GFX.InterpolateCallback(SFX, GFX.add_delay(0.2, GFX.PLAYER), "_defeat")
	GFX.InterpolateCallback(WinLoseNotification, GFX.TotalDelay(GFX.PLAYER), "set_text", "YOU LOSE!")
	GFX.InterpolateCallback(WinLoseNotification, GFX.TotalDelay(GFX.PLAYER), "set_visible", true)

