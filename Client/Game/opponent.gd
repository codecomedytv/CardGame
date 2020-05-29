# warning-ignore:unused_class_variable
var life: int = 8000
var decksize: int = 0
var handsize: int = 0
var field: Array = []
var support: Array = []
var discard: Array = []
var visual
var cards: Dictionary = {}
var opponent: Object
var link: Array = []

func setup(_cards: Dictionary = {}) -> void:
	cards = _cards
	
func show_attack(args) -> void:
	var attacker = cards[args[0]]
	var defender = cards[args[1]] if args[1] != -1 else args[1]
	visual.show_attack(attacker, defender)

func attack_unit(args: Array) -> void:
	var attacker: Object = cards[args[0]]
	var defender: Object = cards[args[1]]
	visual.attack_unit(attacker, defender)
	
func attack_directly(args: Array) -> void:
	var attacker: Object = cards[args[0]]
	visual.attack_directly(attacker)
	
func deploy(args: Array) -> void:
	var data: Dictionary = args[0]
	var card: Object = ClientLibrary.fetch(data)
	card.id = data.id
	cards[card.id] = card
	handsize -= 1
	field.append(card)
	visual.deploy(args)
	
func bounce(args: Array) -> void:
	var card: Object = cards[args[0]]
	field.erase(card)
	handsize += 1
	cards.erase(card.id)
	visual.bounce(card)
	
func resolve() -> void:
	var linked: Array = []
	for card in link:
		if not card.under_players_control:
			linked.append(card)
	visual.resolve(linked)
	
func activate(args: Array) -> void:
	# Some cards may be triggered effects of already existing so we may use an id
	# check first, then default to creating a new instance?
	var card: Object = ClientLibrary.fetch(args[0])
	card.zone = card.Zone.SUPPORT
	var old = support.pop_back()
	# Should probably remove this discard thing and set it on resolve instead
	discard.append(card)
	card.zone = card.Zone.DISCARD
	var targets: Array = []
	if args.size() == 2:
		for id in args[1]:
			targets.append(cards[id])
	visual.activate(card, link, targets)
	
func set_facedown(cards) -> void:
	handsize -= 1
	var blank: Object = ClientLibrary.placeholder()
	support.append(blank)
	visual.set_facedown()
	
func draw(args: Array) -> void:
	var count: int = args[0]
	handsize += count
	decksize -= count
	visual.draw(args, self)

func destroy_unit(args: Array) -> void:
	var id: int = args[0]
	var card: Object = cards[id]
	field.erase(card)
	discard.append(card)
	visual.destroy_unit(args)
	
func lose_life(args: Array) -> void:
	life -= args[0]
	visual.lose_life(args)

func load_deck(args: Array) -> void:
	var _decksize: int = args[0]
	decksize = _decksize
	visual.load_deck(args)
