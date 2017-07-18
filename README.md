<h1>RoseHFSM</h1>
This project is used for the implemention of <a href="https://en.wikipedia.org/wiki/UML_state_machine#Hierarchically_nested_states">hierarchical state-machine</a> AI in <a href="https://Unity3d.com">Unity</a>.

<h2>Features:</h2>
<ul>
<li>An easy to use node editor for designers to develop the behaviours for agents.</li>
<li>A frame work for designers/programmers to inherent from to create their own states specific to the game.</li>
<li>Abstract enough to be used alongside game systems not related to agent behaviour.</li>
</ul>
<h2>Installation:</h2>
Place the files within an appropriate place in your Unity Projects' file structure.</br>
</br>
<h2>How to Use:</h2>
Add a 'Behaviour' object to a game object. Once added you'll be able to click the 'Node Editor' button to 
begin designing agent behaviours.</br>

<h3>States:</h3>
Within the node editor, drag and drop any monoscript files that inherit from the State class to add states. 
Once a State is clicked, variables that you have marked as a SerializeField will appear for you change to 
match the design of your AI. </br>
</br>
<h3>Transitions:</h3>
You may right-click a state to add transitions between them, to edit a state 
click on either of the states that the transition is between and find the button to edit the appropriate 
transition at the bottom of the state editing menu.</br> 
</br>
You have the ability to transition to parent-states and choose where the parent state begins, or to an 
exact state within the parent state if you select another tab while selecting the end of your transition.</br>
Read the parent state section below for more details.</br>
</br>
<h3>Conditions:</h3>
Once editing a transition you may drag and drop monoscripts 
that inherit from a condition class. Similar to the State class, it will show fields you've marked as 
a SerializeField to edit.</br>
</br>
There are built-in distinct boolean conditions that may help you build more 
advanced behaviour:</br> 
<ul>
<li>NOT Condition</li>
<li>AND Condition</li>
<li>OR Condition</li>
</ul>
<h3>Parent State</h3>
A parent state is where more enhanced behaviour may be used. It's basically a state which acts like 
another finite state machine. Right-click anywhere in the editor and select 'Parent State' to add a 
new Parent State. A tab will appear at the top of the node editor so you may switch to editing this
parent state. Once editing this parent state you may either click the 'Base' tab or 'Back' tab to 
return to the last state-machine you were in.</br>
</br>
Read the Transitions sections to read about the more distinct properties of transitioning to a parent
state.</br>
</br>
<h2>Bugs</h2>
<ul>
<li>Currently UnityEvents do not function properly as a SerializeField as their variables will not be saved.</li>
<li>Various errors while switching between GameObjects in the middle of using the NodeEditor.</li>
</ul>
