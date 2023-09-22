tool
extends "res://addons/dialogic/Editor/Events/Parts/EventPart.gd"
 # has an event_data variable that stores the current data!!!

 ## node references
 # e.g. 
onready var char_filepicker = $FilePicker

 # used to connect the signals
func _ready():
	# e.g. 
	char_filepicker.connect("data_changed", self, "_on_filepicker_data_changed")
	pass

 # called by the event block
func load_data(data:Dictionary):
	# First set the event_data
	.load_data(data)
	
	char_filepicker.load_data(event_data) 
	

 # has to return the wanted preview, only useful for body parts
func get_preview():
	return ''

 ## EXAMPLE CHANGE IN ONE OF THE NODES
func _on_filepicker_data_changed(d):
	event_data = d
	data_changed()
