tool
extends "res://addons/dialogic/Editor/Events/Parts/EventPart.gd"
 # has an event_data variable that stores the current data!!!

 ## node references
 # e.g. 
onready var xinput = $HBoxContainer2/XInput
onready var yinput = $HBoxContainer2/YInput
onready var durationinput = $HBoxContainer4/DurationInput
onready var relativecheck = $HBoxContainer3/RelativeCheckBox

 # used to connect the signals
func _ready():
	xinput.connect("value_changed", self, "on_xchanged")
	yinput.connect("value_changed", self, "on_ychanged")
	durationinput.connect("value_changed", self, "on_durationchanged")
	relativecheck.connect("toggled", self, "on_relativechanged")

 # called by the event block
func load_data(data:Dictionary):
	# First set the event_data
	.load_data(data)
	
	# Now update the ui nodes to display the data. 
	# e.g. 
	xinput.value = event_data['posx']
	yinput.value = event_data['posy']
	durationinput.value = event_data['duration']
	relativecheck.pressed = event_data.get('is_relative', true)

 # has to return the wanted preview, only useful for body parts
func get_preview():
	return ''

 ## EXAMPLE CHANGE IN ONE OF THE NODES
func on_xchanged(value):
	event_data['posx'] = value
	data_changed()


func on_ychanged(value):
	event_data['posy'] = value
	data_changed()


func on_durationchanged(value):
	event_data['duration'] = value
	data_changed()


func on_relativechanged(value):
	event_data['is_relative'] = value
	data_changed()
