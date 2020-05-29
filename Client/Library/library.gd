class_name ClientLibrary

const CARD_Object: PackedScene = preload("card/object.tscn")

static func fetch(card: Dictionary) -> Control:
	var data = _archive()[card.setcode].new()
	var visual = CARD_Object.instance()
	visual.id = card.id
	visual.set_data(data)
	return visual
	
static func placeholder() -> Control:
	var card = CARD_Object.instance()
	card.blank = true
	return card

static func _archive() -> Dictionary:
	return {
	code.ALPHA_DungeonGuide: preload("alpha/DungeonGuide.gd"),
	code.ALPHA_QuestReward: preload("alpha/QuestReward.gd"),
	code.ALPHA_CounterAttack: preload("alpha/CounterAttack.gd"),
	code.ALPHA_WrongWay: preload("alpha/WrongWay.gd"),
	code.ALPHA_GuardPuppy: preload("alpha/GuardPuppy.gd"),
	code.ALPHA_TrainingTrainer: preload("alpha/TrainingTrainer.gd"),
	code.ALPHA_NoviceArcher: preload("alpha/NoviceArcher.gd")
}

