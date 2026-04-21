<h2> Study System </h2>
<p>
  Currently a huge WIP.
  The core idea around this is to basically create something similar to the app "Anki", but customizable for things I specifically 
  wanted (from using anki myself) that couldn't be done that way, so here I am, building it myself.
  <br>
</p>

<h2> Main Screen.1 </h2>
<p>
  <img width="1919" height="1032" alt="MainScreen" src="https://github.com/user-attachments/assets/cc5cd2ad-8144-456d-9103-7f19aba10a92" />
^ This is the HomeScreen page that you load into.
  <br>
</p>

<h2> MainScreen.2 </h2>
<p>
  Speaking of the HomeScreen, this little "Create Deck (Template)" button,
  this is temporary (Tooltip even says so).
  The point of this button is to create a template for the Deck object for testing with and such.
  You press the button, it generates a TemplateDeck.jcard file (The template deck) over in,
  %appdata% (C:\Users\*User*\AppData\Roaming\StudySystem\Decks\TemplateDeck.jcard)
  <br><br><br>
</p>

<h2> Settings Screen </h2>
<p>
  <img width="1916" height="1032" alt="SettingsScreen02" src="https://github.com/user-attachments/assets/766ed6dd-f46c-4af4-8f14-e6145176f2ba" />
^ SettingsScreen - Back button,
  which brings you to HomeScreen.
  --- Since V0.2.0 ---
  - Have began work on the to come layout area for where buttons shall exist
  <br><br><br>
</p>

<h2> Study Screen </h2>
<p>
  <img width="1919" height="1032" alt="StudyScreen" src="https://github.com/user-attachments/assets/332a3e74-be5f-4d2e-88fb-c6ace7be7937" />
^ This is the "StudyScreen",
  Allows you to load a deck (That dropdown box is used to select the deck), that + box is "ImportDeck", If one exists already.
  Will populate the ComboBox after importing is done, and user clicks the dropdown box on right.
  Then the "Back" button just brings to "HomeScreen".
  <br><br><br>
</p>

<h2> Deck Editor || Builder Screen </h2>
<p>
  <img width="1918" height="1028" alt="DeckEditScreen_004" src="https://github.com/user-attachments/assets/aea4c8b2-08c6-4f4b-a2da-a27c042c7e54" />
^ This is the "BuilderScreen"
  - Modified layout some more

  You can see I filled in the boxes on upper left side with text, and it displays the text in the 'CardLayout' format to the right (the center of the screen)
  The 'Answer' field, actually has a Label showing "Answer:" above where the actual Text lives.
  <br><br><br>
  Buttons:<br>
  - Home - Brings to main screen<br>
  - Previous Card - Shows the previous Card in Deck. If exists<br>
  - Next Card - Shows the next card in Deck. If exists<br>
  - Add Card - Adds new Card after currently selected Card<br>
  - Remove Card - Removes currently selected Card from the selected Deck<br>
  - Save Deck - Saves currently selected deck
    <br><br>
  Combo Boxes:<br>
  - Decks - Populates current Decks<br>
  - Cards - Populates current Card within said Deck
    <br><br><br>
  Text Boxes:<br>
    Front + Reading + Extras + Pronunciation + Answer<br>All of these populate
    the respective fields of the card, showing a display to the right of what the card will look like.
  - These Text Box fields can be modified, they automatically update in memory, "Save Deck" button must be used or changes will not save.
  </p>
<h2> Card View Screen </h2>
<p>
  <img width="1919" height="1032" alt="CardScreen001" src="https://github.com/user-attachments/assets/c71b645c-1e52-420a-92c2-678fb90a92ed" />
^ This is the "CardScreen",
  The main study loop.
  <br><br>
  Once you load a deck on prior "StudyScreen", this now shows the first card in that deck in the middle of the screen.
  You then select "how difficult this is", using the 3 colored buttons for "Hard", "Normal", and "Easy".
  Once you have chose 1 of the 3 options, that "Answer" button will be enabled and visible fully,
  rather than the lighter look it has prior.
  - Have begun working in a simplified version of an SRS logic for studying based on the answer for difficulty you provide.
</p>
