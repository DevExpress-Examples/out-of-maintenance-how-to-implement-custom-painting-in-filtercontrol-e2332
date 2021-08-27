<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128621186/11.2.5%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E2332)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
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


