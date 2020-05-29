extends TextureRect


var playing = true
var frame: int = 0
const MAX_FRAME: int = 25
var frames = []

func _ready():
	var d = Directory.new()
	var path: String = "res://assets/spritesheets/S-PNg/1"
	var err = d.open(path)
	if err != OK:
		push_warning(err as String)
	d.list_dir_begin(true)
	var filename = d.get_next()
	while filename != "":
		if filename.ends_with(".png"):
			frames.append(load("%s/%s" % [path, filename]))
		filename = d.get_next()
	d.list_dir_end()
	
func _process(delta):
	if playing:
		next_frame()
		
func next_frame() -> void:
	frame += 0.5
	if frame == MAX_FRAME:
		frame = 0
	texture = frames[frame]
