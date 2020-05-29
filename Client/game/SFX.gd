extends AudioStreamPlayer

# Most of these should be callbacks from the animation manager

const DEPLOY_SFX: AudioStream = preload("res://assets/sounds/Rune.wav")
const CARD_DRAW_SFX: AudioStream = preload("res://assets/sounds/Card_Game_Movement_Deal_Single_01.wav")
const SET_SUPPORT_SFX: AudioStream = preload("res://assets/sounds/Card_Game_Play_Swirl_Wind_01.wav")
const BATTLE_UNIT_SFX: AudioStream = preload("res://assets/sounds/Card_Game_Action_Stomp_01.wav")
const VICTORY_SFX: AudioStream = preload("res://assets/sounds/BRPG_Victory_Stinger.wav")
const DEFEAT_SFX: AudioStream = preload("res://assets/sounds/BRPG_Defeat_Stinger.wav")

func _deploy() -> void:
	stream = DEPLOY_SFX
	play()
	
func _set_support() -> void:
	stream = SET_SUPPORT_SFX
	play()
	
func _draw_card() -> void:
	stream = CARD_DRAW_SFX
	play()
	
func _battle_unit() -> void:
	stream = BATTLE_UNIT_SFX
	play()

func _victory() -> void:
	stream = VICTORY_SFX
	play()
	
func _defeat() -> void:
	stream = DEFEAT_SFX
	play()
