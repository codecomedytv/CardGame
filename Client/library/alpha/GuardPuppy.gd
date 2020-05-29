extends cCard.cUnit

func _init() -> void:
	title = "Guard Puppy"
	text = "Cannot be destroyed by battle.\nYour opponent cannot attack other untis"
	setcode = code.ALPHA_GuardPuppy
	attack = 500
	defense = 500
	illustration = "res://assets/CardArt/boar.png"
