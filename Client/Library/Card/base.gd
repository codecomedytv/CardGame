extends Reference
class_name cCard

class Base extends Reference:
	# warning-ignore:unused_class_variable
	var title: String = "Card"
	# warning-ignore:unused_class_variable
	var setcode: int = 0
	# warning-ignore:unused_class_variable
	var id: int
	# warning-ignore:unused_class_variable
	var owner: Reference
	# warning-ignore:unused_class_variable
	var controller: Reference
	# warning-ignore:unused_class_variable
	var illustration
	var text: String = ""
	
class cUnit extends Base:
	# warning-ignore:unused_class_variable
	const CARD_TYPE = CARD_TYPE.UNIT
	# warning-ignore:unused_class_variable
	var attack: int = 0
	# warning-ignore:unused_class_variable
	var defense: int = 0
	
class cSupport extends Base:
	# warning-ignore:unused_class_variable
	const CARD_TYPE = CARD_TYPE.SUPPORT
	# warning-ignore:unused_class_variable
	var default_draw: int = 0
