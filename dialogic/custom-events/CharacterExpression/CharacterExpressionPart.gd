tool
extends "res://addons/dialogic/Editor/Events/Parts/EventPart.gd"
 # has an event_data variable that stores the current data!!!

 ## node references
 # e.g. 
onready var name_input = $HBoxContainer/NameInput
onready var expression_option : OptionButton = $HBoxContainer2/ExpressionOption
onready var duration_input = $HBoxContainer3/Duration

 # used to connect the signals
func _ready():
	# e.g. 
	expression_option.clear()
	expression_option.add_item("Angry", 0);
	expression_option.add_item("Calm", 1);
	expression_option.add_item("Excited", 2);
	expression_option.add_item("Happy", 3);
	expression_option.add_item("Heart", 4);
	expression_option.add_item("Inspiration", 5);
	expression_option.add_item("Music", 6);
	expression_option.add_item("Poker", 7);
	expression_option.add_item("Surprised", 8);
	expression_option.add_item("Wrong", 9);
	name_input.connect("text_changed", self, "_on_NameInput_text_changed")
	expression_option.connect("item_selected", self, "_on_ExpressionOption_selected")
	duration_input.connect("value_changed", self, "_on_Duration_changed")
	pass

 # called by the event block
func load_data(data:Dictionary):
	# First set the event_data
	.load_data(data)
	
	# Now update the ui nodes to display the data. 
	# e.g. 
	name_input.text = event_data['name']
	duration_input.value = event_data['duration']
	var selectedid : int
	match(event_data['expression']):
		"Angry":
			selectedid = 0
		"Calm":
			selectedid = 1
		"Excited":
			selectedid = 2
		"Happy":
			selectedid = 3
		"Heart":
			selectedid=4
		"Inspiration":
			selectedid=5
		"Music":
			selectedid = 6
		"Poker":
			selectedid = 7
		"Surprised":
			selectedid = 8
		"Wrong":
			selectedid = 9
			
	var selectedindex = expression_option.get_item_index(selectedid)
	
	expression_option.select(selectedindex)

 # has to return the wanted preview, only useful for body parts
func get_preview():
	return ''

 ## EXAMPLE CHANGE IN ONE OF THE NODES
func _on_InputField_text_changed(text):
	event_data['my_text_key'] = text
	
	# informs the parent about the changes!
	data_changed()

func _on_NameInput_text_changed(text):
	event_data['name'] = text
	
	data_changed()

func _on_ExpressionOption_selected(id):
	event_data['expression'] = expression_option.get_item_text(id)
	
	data_changed()

func _on_Duration_changed(value):
	event_data['duration'] = value
	data_changed()
