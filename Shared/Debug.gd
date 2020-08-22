extends Node

func _input(event):
    if event is InputEventKey and event.is_pressed():
        if event.scancode == KEY_F:
           OS.window_fullscreen = !OS.window_fullscreen
        elif event.scancode == KEY_Q:
            get_tree().quit()