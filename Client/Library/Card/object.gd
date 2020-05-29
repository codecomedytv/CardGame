extends Control

enum Zone {
	DECK
	DISCARD
	HAND
	UNIT
	SUPPORT
	}
# Data
var id: int = 0
var title: String
var illustration: String
var attack: int = 0
var defense: int = 0
var effect: String
var card_type: int
var ready: bool = false
var blank: bool = false
var legal: bool = false
var zone: int = Zone.DECK setget _zone_change
var under_players_control: bool = false
var type: String = "None"
var valid_targets: Array = []
var selected_targets: Array = []
var activated: bool = false
var can_be_deployed: bool = false
var can_be_set: bool = false
var can_be_activated: bool = false
var can_attack: bool = false
var player: Reference

# Set by the game
var interact: AnimatedSprite

# Visual
onready var LegalPlay: AnimatedSprite = $Frame/LegalPlay
onready var Link: Label = $Link
onready var ID: Label = $ID
onready var Title: Label = $Title
onready var Attack: Label = $Battle/Attack
onready var Defense: Label = $Battle/Defense
onready var Frame: Sprite = $Frame
onready var Illustration: Sprite = $Frame/Illustration
onready var Back: TextureRect = $Back
onready var Rest: Panel = $Rest
onready var Sound: AudioStreamPlayer = $AudioStreamPlayer
onready var ValidTarget: AnimatedSprite = $Frame/ValidTarget
onready var SelectedTarget: AnimatedSprite = $Frame/Selected
onready var Combat: TextureRect = $Combat

func show_as_valid(yes: bool) -> void:
	if yes:
		ValidTarget.show()
		ValidTarget.play()
	else:
		ValidTarget.hide()
		ValidTarget.stop()
	
func show_as_targeted() -> void:
	SelectedTarget.show()
	SelectedTarget.play()
	
func remove_aura() -> void:
	ValidTarget.hide()
	SelectedTarget.hide()
	ValidTarget.stop()
	SelectedTarget.stop()

func _zone_change(_zone: int) -> void:
	zone = _zone
#	if is_inside_tree():
#		Frame.z_index += 1

func _to_string() -> String:
	return "[ %s : %s ]" % [id, title]

func ready() -> void:
	Rest.visible = false
	
func exhaust() -> void:
	Rest.visible = true
	
func does_not_require_targets() -> bool:
	return valid_targets.empty()
	
func legalize() -> void:
	legal = true
	glow()
	
var c: int = 0

func set_z(value) -> void:
	if c + value < -1:
		return
	elif c + value > 1:
		return
	else:
		c + value
		Frame.z_index += value

func glow() -> void:
	if legal and get_global_rect().has_point(get_global_mouse_position()) and zone == Zone.HAND:
		Sound.play()
		LegalPlay.visible = true
		LegalPlay.play()
		return
	LegalPlay.visible = false
	LegalPlay.stop()
	
func turn_invisible() -> void:
	modulate.a = 0.0
	
func turn_visible() -> void:
	modulate.a = 1.0

func flip_face_up() -> void:
	Back.visible = false
	
func flip_face_down() -> void:
	Back.visible = true
	
func set_data(data: Object) -> void:
	title = data.title
	effect = data.text
	card_type = data.CARD_TYPE
	illustration = data.illustration
	if card_type == CARD_TYPE.UNIT:
		attack = data.attack
		defense = data.defense
	
func visualize() -> void:
	if blank:
		flip_face_down()
		return
	ID.text = id as String
	Title.text = title
	Illustration.texture = load(illustration)
	if card_type == CARD_TYPE.UNIT:
		Attack.text = attack as String 
		Defense.text = defense as String
	else:
		$Battle.hide()
	# Include Some Support Stuff Here?
	
func select() -> void:
	pass

func _ready() -> void:
	visualize()
	

	
func _on_Card_mouse_entered():
	glow()
	
func _on_Card_mouse_exited():
	Sound.stop()
	glow()

func _pressed():
	if interact == null:
		print("Interact is Gone")
		return
	if interact.card != null:
		print("A Card is already being interacted with")
		return
	match zone:
		Zone.HAND:
			if can_be_deployed or can_be_set:
				interact.add(self)
		Zone.UNIT:
			print("Attempting Attack")
			if can_attack:
				print("Can Attack")
				interact.add(self)
			else:
				print("Cannot Attack")
		Zone.SUPPORT:
			if can_be_activated:
				activate()
#			# Slightly more complicated
#				pass

#func activate(card: Object, skill_index: int = 0, targets: Array = []) -> void:
#	# If we have selected no targets while we have targets to select and we're not currently selecting a target
#	# then lets animate and select a target
#	if card.activated:
#		return
#	if targets.empty() and not card.valid_targets.empty():
#		_selecting_target = true
#		player.activate([card.id])
#		player.visual.animate.start()
#		_show_valid_targets(card)
#		return
#	elif card.does_not_require_targets():
#		player.activate([card.id])
#		player.visual.animate.start()
#	card.activated = true
#	card.legal = false
#	call_deferred("_activate", card, skill_index, targets)
#
#	_stop_targeting(card)
	
signal activated
func activate(skill: int = 0, targets: Array = []) -> void:
	player.activate([id])
	player.visual.animate.start()
	legal = false
	call_deferred("emit_signal", "activated", self, skill, targets)
