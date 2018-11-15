<!-- default file list -->
*Files to look at*:

* [Form1.cs](./CS/Q264421/Form1.cs) (VB: [Form1.vb](./VB/Q264421/Form1.vb))
* [MyFilterControl.cs](./CS/Q264421/MyFilterControl.cs) (VB: [MyFilterControl.vb](./VB/Q264421/MyFilterControl.vb))
<!-- default file list end -->
# How to implement custom painting in FilterControl


<p>This example demonstrates how to create the FilterControl descendant, and raise the event, allowing you to draw each label separately.</p>


<h3>Description</h3>

<p>In version 2011 vol 2, the FilterControl class was refactored, and this example was significantly changed. Specifically, we have removed a custom nodes factory and custom nodes, because custom FilterControlLabelInfo is not created in the new MyWinFilterTreeNodeModel class.</p>

<br/>


