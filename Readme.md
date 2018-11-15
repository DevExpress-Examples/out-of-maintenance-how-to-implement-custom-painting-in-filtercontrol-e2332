<!-- default file list -->
*Files to look at*:

* [Form1.cs](./CS/Q264421/Form1.cs) (VB: [Form1.vb](./VB/Q264421/Form1.vb))
* [MyFilterControl.cs](./CS/Q264421/MyFilterControl.cs) (VB: [MyFilterControl.vb](./VB/Q264421/MyFilterControl.vb))
<!-- default file list end -->
# How to implement custom painting in FilterControl


<p>This example demonstrates how to create the FilterControl descendant, and raise the event, allowing you to draw each label separately.</p>


<h3>Description</h3>

<p>Starting from version 2011 vol 1, the signature of the Node.SetOwner method was modified due to the refactoring. Now, the second parameter should be of the Node type.<br />
Starting from version 2011 vol 1, the NodeElement class was renamed to NodeEditableElement due to the refactoring. <br />
Starting from version 2011 vol 1, the NodeElement.Type property was renamed to ElementType due to the refactoring, and has become a member of the NodeEditableElement class. </p>

<br/>


