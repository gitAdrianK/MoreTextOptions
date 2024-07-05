Gives you the ability to customize text and outline colour of ingame strings like the timer or the ones added by other mods like Level%.

Information for map makers:
To change the colour of a maps credits, dialog, or location text (that you have access to the xml file containing the text to) add
`{color="#RRGGBB"}`
to the front of your text, the colour will affect all text to the right of it, another color tag can be used to change it again, don't place it at the end though, replace the RRGGBB with a valid hex representation of a colour, e.g. 00FF00 for full green. While technically you should be able to use this in other places too it seems to not play nicely all the time, so testing what works and what doesn't is on your end.
For example when a dialog is split to the next line it loses its colour, the only (to me) known workaround is to apply the colour again.

Information for other modders:
For your own mod to use custom colours use the function TextHelper.DrawString, if you dont want your text to be changed use Game1.spriteBatch.DrawString

Was "Less", but with the 1.1 feature to customize outline colour "More" is more approriate.
Was "Outlines", but with the 1.2 feature to customize text colour "Options" is more approriate.
SpriteFonts are a pain in the ass and changing Font could lead to not supported characters, scaling leads to artifacts, so quite frankly colour is the only viable thing to change, which really would have me rebrand this from "Options" to "Colours" but I won't, whatever.

[Mod](https://steamcommunity.com/sharedfiles/filedetails/?id=3275249832)
