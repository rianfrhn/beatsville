tool
extends "res://addons/dialogic/Editor/Events/Parts/EventPart.gd"
 # has an event_data variable that stores the current data!!!

 ## node references
 # e.g. 
onready var cb = $CheckBox

 # used to connect the signals
func _ready():
	# e.g. 
	cb.connect("toggled", self, "_onToggled")
	pass

 # called by the event block
func load_data(data:Dictionary):
	# First set the event_data
	.load_data(data)
	
	# Now update the ui nodes to display the data. 
	# e.g. 
	cb.pressed = event_data.get('enable_movement', true)

 # has to return the wanted preview, only useful for body parts
func get_preview():
	return ''

 ## EXAMPLE CHANGE IN ONE OF THE NODES
func _onToggled(button_pressed):
	event_data['enable_movement'] = button_pressed
	
	# informs the parent about the changes!
	data_changed()
