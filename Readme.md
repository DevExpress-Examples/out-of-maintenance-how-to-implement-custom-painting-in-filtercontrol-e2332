# How to implement custom painting in FilterControl


<p>This example demonstrates how to create the FilterControl descendant, and raise the event, allowing you to draw each label separately.</p>


<h3>Description</h3>

<p>In version 2011 vol 2, the FilterControl class was refactored, and this example was significantly changed. Specifically, we have removed a custom nodes factory and custom nodes, because custom FilterControlLabelInfo is not created in the new MyWinFilterTreeNodeModel class.</p>

<br/>


