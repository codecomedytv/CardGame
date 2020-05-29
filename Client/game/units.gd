extends HBoxContainer

var interact

func _ready() -> void:
#	var index: int = 0
	for child in get_children():
		child.connect("pressed", self, "deploy", [child.name as int])
	interact = get_tree().get_nodes_in_group("interact").pop_front()
		
func deploy(zone: int) -> void:
	print("deploying to %s" % zone as String)
	if interact.card != null and interact.card.can_be_deployed:
		interact.card.deploy(zone)
		print("deploying %s to %s" % [interact.card, zone as String])
