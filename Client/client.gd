extends Connection
class_name Client

### BEGIN PROTOTYPE USER ###
# Information used until we expand user profiles
# We could probably keep something like this as a default profile
var decklist: Array = []
export(bool) var pause_sound: bool = false
export(String, "res://client/game/manual.gd", "res://client/game/autobot.gd") var input = "res://client/game/manual.gd"
#const Error: PackedScene = preload("Error.tscn")
const Error: Script = preload("Error.gd")
const GAME = preload("res://client/game/Game.tscn")
const _IP: String = "127.0.0.1"
const _PORT: int = 5000
var _client: NetworkedMultiplayerENet
var game

func default_deck() -> Array:
	var _decklist: Array = []
	for i in 34:
		_decklist.append(code.ALPHA_DungeonGuide)
	_decklist.append(code.ALPHA_GuardPuppy)
	_decklist.append(code.ALPHA_WrongWay)
	_decklist.append(code.ALPHA_CounterAttack)
	_decklist.append(code.ALPHA_QuestReward)
	_decklist.append(code.ALPHA_NoviceArcher)
	_decklist.append(code.ALPHA_TrainingTrainer)
	return _decklist
	
func _ready() -> void:
	$Join.connect("pressed", self, "join")

func join(_decklist: Array = default_deck()) -> void:
	remove_child($Join)
	decklist = _decklist
	_client = NetworkedMultiplayerENet.new()
	var err = _client.create_client(_IP, _PORT)
	if err != OK: push_warning(err as String)
	err = multiplayer.connect("connected_to_server", self, "_register_on_server")
	if err != OK: push_warning(err as String)
	multiplayer.set_network_peer(_client)
	
func _force_disconnect(reason) -> void:
	_client.close_connection()
	remove_child(game)
	add_child(Error.new(reason))

func _register_on_server() -> void:
	rpc_id(1, "_register_player", multiplayer.get_network_unique_id(), decklist)

puppet func _create_room(game_id: String) -> void:
	game = GAME.instance()
	game.name = game_id
	add_child(game)
	game.setup(pause_sound, load(input).new())
