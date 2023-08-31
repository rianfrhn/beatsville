extends Node


func handle_event(event_data, dialog_node):
	""" 
		If this event should wait for dialog advance to occur, uncomment the WAITING line
		If this event should block the dialog from continuing, uncomment the WAITINT_INPUT line
		While other states exist, they generally are not neccesary, but include IDLE, TYPING, and ANIMATING
	"""
	#dialog_node.set_state(dialog_node.state.WAITING)
	#dialog_node.set_state(dialog_node.state.WAITING_INPUT)
	
	pass # fill with event action
	var npc = event_data['name']
	var x = event_data['posx']
	var y = event_data['posy']
	var isRelative = event_data.get('is_relative', true)
	
	# once you want to continue with the next event
	WorldManipulator.MoveNPC(npc, x, y, isRelative)
	dialog_node._load_next_event()
	dialog_node.set_state(dialog_node.state.READY)
