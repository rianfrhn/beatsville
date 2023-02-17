tool
extends "res://addons/dialogic/Editor/Events/Parts/EventPart.gd"
 # has an event_data variable that stores the current data!!!

 ## node references
 # e.g. 
onready var input_player1 = $HBoxContainer4/Player1Res
onready var input_player2 = $HBoxContainer4/Player2Res
onready var input_songdata = $HBoxContainer5/SongDataRes
onready var bossfight_check = $HBoxContainer/BossFightCheckBox
onready var xpgain_ammount = $HBoxContainer2/XPGain
onready var moneygain_ammount = $HBoxContainer2/MoneyGain

 # used to connect the signals
func _ready():
	# e.g. 
	input_player1.connect("text_changed", self, "_on_player1_text_changed")
	input_player2.connect("text_changed", self, "_on_player2_text_changed")
	input_songdata.connect("text_changed", self, "_on_songdata_text_changed")
	
	
	xpgain_ammount.connect("value_changed", self, "_on_xpgain_value_changed")
	moneygain_ammount.connect("value_changed", self, "_on_moneygain_value_changed")
	bossfight_check.connect("toggled", self, "_on_isbossfight_toggled")
	pass

 # called by the event block
func load_data(data:Dictionary):
	# First set the event_data
	.load_data(data)
	
	# Now update the ui nodes to display the data. 
	# e.g. 
	input_player1.text = event_data['player1']
	input_player2.text = event_data['player2']
	input_songdata.text = event_data['song_data']
	xpgain_ammount.value = event_data['xp_gain']
	moneygain_ammount.value = event_data['money_gain']
	bossfight_check.pressed = event_data.get('is_bossfight', true)

 # has to return the wanted preview, only useful for body parts
func get_preview():
	return ''

 ## EXAMPLE CHANGE IN ONE OF THE NODES
func _on_player1_text_changed(text):
	event_data['player1'] = text
	
	# informs the parent about the changes!
	data_changed()
	
func _on_player2_text_changed(text):
	event_data['player2'] = text
	
	# informs the parent about the changes!
	data_changed()
	
func _on_songdata_text_changed(text):
	event_data['song_data'] = text
	
	# informs the parent about the changes!
	data_changed()
	
func _on_xpgain_value_changed(value):
	event_data['xp_gain'] = value
	data_changed()

func _on_moneygain_value_changed(value):
	event_data['money_gain'] = value
	data_changed()

func _on_isbossfight_toggled(checkbox_value):
	event_data['is_bossfight'] = checkbox_value
	data_changed()
