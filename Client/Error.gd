extends Label

var reason: int = 0

func _init(_reason: int) -> void:
	reason = _reason
	text = "DISCONNECTED\n(because %s)" % reason as String

func _ready():
	rect_size = get_tree().root.get_viewport().size
	align = Label.ALIGN_CENTER
	valign = Label.ALIGN_CENTER
