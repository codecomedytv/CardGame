extends AnimatedSprite

var card: Object

func add(_card: Object) -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_HIDDEN)
	card = _card
	visible = true
#
func stop() -> void:
	visible = false
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	card.LegalPlay.stop()
	card.LegalPlay.hide()
	card = null
#
func _process(delta: float) -> void:
#	# Always processing, just invisible sometimes!
	position = get_global_mouse_position()
	if card != null and not card.LegalPlay.visible:
		card.LegalPlay.play()
		card.LegalPlay.show()
