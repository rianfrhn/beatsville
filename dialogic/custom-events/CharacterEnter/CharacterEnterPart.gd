tool
extends "res://addons/dialogic/Editor/Events/Parts/EventPart.gd"
 # has an event_data variable that stores the current data!!!

 ## node references
onready var exit_bool = $BoolContainer/ExitInstead
onready var enter_container = $EnterContainer
onready var exit_container = $ExitContainer

onready var char_filepicker = $EnterContainer/Char/FilePicker
onready var xinput = $EnterContainer/Coords/X
onready var yinput = $EnterContainer/Coords/Y
onready var faceleft_bool = $EnterContainer/BoolContainer/FaceLeft

onready var name_input = $ExitContainer/NameInput

 # used to connect the signals
func _ready():
	
	exit_bool.connect("toggled", self, "_on_exit_changed")
	char_filepicker.connect("data_changed", self, "_on_filepicker_data_changed")
	xinput.connect("value_changed", self, "_on_xinput_changed")
	yinput.connect("value_changed", self, "_on_yinput_changed")
	faceleft_bool.connect("toggled", self, "_on_faceleft_changed")
	
	name_input.connect("text_changed", self, "_on_nameinput_changed")
	pass

 # called by the event block
func load_data(data:Dictionary):
	# First set the event_data
	.load_data(data)
	
	# Now update the ui nodes to display the data. 
	# e.g. 
	exit_bool.pressed = event_data.get('exit', true)
	exit_container.visible = event_data.get('exit', true)
	enter_container.visible = !event_data.get('exit', true)
	
	char_filepicker.load_data(event_data)
	xinput.value = event_data["xpos"]
	yinput.value = event_data["ypos"]
	faceleft_bool.pressed = event_data.get('faceleft', true)
	
	name_input.text = event_data["npc_name"]
	
	
func get_preview():
	return ''

 ## EXAMPLE CHANGE IN ONE OF THE NODES
func _on_exit_changed(b):
	exit_container.visible = b
	enter_container.visible = !b
	event_data['exit'] = b
	data_changed()

func _on_filepicker_data_changed(d):
	event_data = d
	data_changed()

func _on_xinput_changed(v):
	event_data['xpos'] = v
	data_changed()

func _on_yinput_changed(v):
	event_data['ypos'] = v
	data_changed()

func _on_nameinput_changed(v):
	event_data['npc_name'] = v
	data_changed()
	
