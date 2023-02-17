extends Node


func handle_event(event_data, dialog_node):
	""" 
		If this event should wait for dialog advance to occur, uncomment the WAITING line
		If this event should block the dialog from continuing, uncomment the WAITINT_INPUT line
		While other states exist, they generally are not neccesary, but include IDLE, TYPING, and ANIMATING
	"""
	#dialog_node.set_state(dialog_node.state.WAITING)
	#dialog_node.set_state(dialog_node.state.WAITING_INPUT)
	
	# fill with event action
	var player1 = event_data['player1']
	var player2 = event_data['player2']
	var songdata = event_data['song_data']
	var isbossfight = event_data.get('is_bossfight', true)
	var moneygain = event_data['money_gain']
	var xpgain = event_data['xp_gain']
	GlobalVariable.InitializeFightData(player1, player2, songdata, isbossfight, moneygain,xpgain)
	
	
	# once you want to continue with the next event
	dialog_node._load_next_event()
	dialog_node.set_state(dialog_node.state.READY)
