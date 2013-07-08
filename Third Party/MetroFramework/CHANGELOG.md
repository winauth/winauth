Changelog File

-----------------------------------------------------------------------------------
SW 2013-04-20 12:36 - AssemblyVersion 1.3.0.0
-----------------------------------------------------------------------------------

* Updated assembly version to 1.3.0.0
* Fixed several bugs
* Enhanced overall drawing behvaiour with custom drawing events for full manipulation support in drawing process
* Every Control supports real transparency now
* Background Image included back into several controls


-----------------------------------------------------------------------------------
SW 2013-04-02 09:55 - AssemblyVersion 1.2.0.0
-----------------------------------------------------------------------------------

* Added support for a style extender control (applys theme propertys to none framework controls)
* Fixed textbox tab stop behaviour
* Fixed maximize window behaviour
* Fixed ComboBox item text by using a data source
* Added additional propertys to textbox

*** thx to thielj, who strongly tested and helped to improve the code! ***


-----------------------------------------------------------------------------------
SW 2013-03-04 17:32 - AssemblyVersion 1.1.0.0
-----------------------------------------------------------------------------------

* Added support for realistic drop shadow
* Implemented "ShadowType" Property to switch between old flat shadow and new realistic drop shadow

*** thx to nripendra, who delivers the base code! ***


-----------------------------------------------------------------------------------
SW 2013-03-03 19:20 - AssemblyVersion 1.1.0.0
-----------------------------------------------------------------------------------

* MetroForm Property Moveable disables the possibility to move the form completely
* MetroTile has new Property "PaintTileCount" to disable the painting of the tile counts
* Basic controls now providing Properties "CustomBackColor" and "CustomForeColor" to override the default metro framework styles
* Added support for multi monitor environments
* Several bug fixes


-----------------------------------------------------------------------------------
SW 2013-02-20 17:03 - AssemblyVersion 1.1.0.0
-----------------------------------------------------------------------------------

* MetroForm has new Property DisableHeader to hide HeaderText
* Scrollbar has new Property ScrollbarSize to manually define Scrollbar thickness
* MetroLocalize now renders default language correctly if localization file could not be found
* MetroTextbox is resizable again
* ProgressSpinner does not spin in DesignMode and Spinning is changed to TRUE as default


-----------------------------------------------------------------------------------
SW 2013-02-20 - AssemblyVersion 1.1.0.0
-----------------------------------------------------------------------------------

* Changed AssemblyVersion
* Refactored ProgressBar and TabControl to fit the default MetroDesign
* Refactored all Controls to be more stable
* Stripped out and cleaned up overall code structure
* Removed DWM Glass Extend on Vista/7 because was to buggy
* Implemented ShadowForm to create a custom shadow for all forms
* Set FormBorderStyle=None as Default on all plattforms
* Implemented new controls: MetroProgressSpinner, MetroComboBox
* Currently under construction: MetroContextMenuStrip
* Fixed Scrollbar Behaviour overall (Problem in TabControl occurs everywhere... The ScrollValue calculation has to be extended to use the ThumbSize also!)
* Implemented translation class (MetroLocalize) to localize used texts in controls (at this moment only for MetroToggle)
* Implemented GripSizeDisplay on MetroForm
* Added support for window startup location

-----------------------------------------------------------------------------------