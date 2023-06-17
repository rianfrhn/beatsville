tool
extends "res://addons/dialogic/Editor/Events/Parts/EventPart.gd"
 # has an event_data variable that stores the current data!!!

 ## node references
 # e.g. 
onready var nameInput = $HBoxContainer/NameInput
onready var xInput = $HBoxContainer2/XInput
onready var yInput = $HBoxContainer2/YInput
onready var relativeInput = $HBoxContainer3/RelativeCheckBox
 # used to connect the signals
func _ready():
	
	
	
	nameInput.connect("text_changed", self, "_on_nameInput_changed")
	xInput.connect("value_changed", self, "_onxInput_changed")
	yInput.connect("value_changed", self, "_onyInput_changed")
	relativeInput.connect("toggled", self, "_onrelativeInput_changed")
	pass

 # called by the event block
func load_data(data:Dictionary):
	# First set the event_data
	.load_data(data)
	
	# Now update the ui nodes to display the data. 
	# e.g. 
	nameInput.text = event_data['name']
	xInput.value = event_data['posx']
	yInput.value = event_data['posy']
	relativeInput.pressed = event_data.get('is_relative', true)

 # has to return the wanted preview, only useful for body parts
func get_preview():
	return ''
	
func _on_nameInput_changed(text):
	event_data['name'] = text
	data_changed()
	
func _onrelativeInput_changed(value):
	event_data['is_relative'] = value
	data_changed()
	
func _onxInput_changed(value):
	event_data['posx'] = value
	data_changed()
	
func _onyInput_changed(value):
	event_data['posy'] = value
	data_changed()
