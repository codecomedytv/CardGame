extends TextureRect

onready var title = $Title
onready var art = $Illustration
onready var label = $Text/Effect
onready var battle = $Text/Unit

func _process(delta) -> void:
	if Input.is_action_just_pressed("view_card") or Input.is_action_just_pressed("drag_drop"):
		view_card()
		
func view_card() -> void:
	for card in get_tree().get_nodes_in_group("cards"):
		if card.get_global_rect().has_point(get_global_mouse_position()):
			title.text = card.title
			if card.illustration != "":
				art.texture = load(card.illustration)
			if card.card_type == CARD_TYPE.UNIT:
				battle.text = "[ %s / ATK %s / DEF %s ]" % ["Warrior", card.attack, card.defense]
			else:
				battle.text = ""
			label.text = card.effect
