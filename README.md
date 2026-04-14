<h2>Study Screen</h2>
<p>
  Currently a huge WIP.
  The core idea around this is to basically create something similar to the app "Anki", but customizable for things I specifically 
  wanted (from using anki myself) that couldn't be done that way, so here I am, building it myself.
  The current V0.1, is just a concept so far, (The later added V0.1.8 has more 'filling') so the basis of what it will eventually be built on.
  Things will change over time as seen fit etc.
  <br>
  <h2>This part below speaks of V0.1 specifically</h2>
  For now though, everything is essentially just hard coded, easy enough to in later versions change it over to load a JSON object for the cards/decks/etc.
  <h2>Everything seen currently, is the V0.1.8 snapshot, progress wise</h2>
  This current version will not remain as is, this does outline the basis of what will be to come though.
  Not to mention, there are many things planned for the future of this, which are not currently even spoke of let alone implemented.
  <br><br><br>
</p>

<h2>Main Screen</h2>
<p>
  <img width="1919" height="1032" alt="MainScreen" src="https://github.com/user-attachments/assets/cc5cd2ad-8144-456d-9103-7f19aba10a92" />
^ This is the HomeScreen page that you load into.
  <br>
</p>

<h2>Extras: MainScreen P2</h2>
<p>
  Speaking of the HomeScreen, this little "Create Deck (Template)" button,
  this is temporary (Tooltip even says so).
  The point of this button is to create a template for the Deck object for testing with and such.
  You press the button, it generates a TemplateDeck.jcard file (The template deck) over in,
  %appdata% (C:\Users\*User*\AppData\Roaming\StudySystem\Decks\TemplateDeck.jcard)
  <br><br><br>
</p>

<h2>Settings Screen</h2>
<p>
  <img width="1918" height="1029" alt="SettingsScreen" src="https://github.com/user-attachments/assets/29e7b795-6fb9-489e-a2b6-470171b5a65e" />
^ "SettingsScreen" - Prime example of "WIP - Work In Progress".
  Nothing done on here besides putting in this one "Back" button,
  which brings you to HomeScreen.
  <br><br><br>
</p>

<h2>Study Screen</h2>
<p>
  <img width="1919" height="1032" alt="StudyScreen" src="https://github.com/user-attachments/assets/332a3e74-be5f-4d2e-88fb-c6ace7be7937" />
^ This is the "StudyScreen",
  Allows you to load a deck (That dropdown box is used to select the deck), that + box is "ImportDeck" if one exists already,
  will populate a new list after importing is done.
  Then the "Back" button just brings to "HomeScreen" once again.
  <br><br><br>
</p>

<h2>Deck Editor</h2>
<p>
  <img width="1918" height="1029" alt="DeckEditScreen_003" src="https://github.com/user-attachments/assets/646384cb-fe16-4d82-b8ed-7d5fb386fbb6" />
^ This is the "DeckEditScreen",
  Where you have many different things available.
  The buttons do nothing at the moment, concept idea of what they will be and what should exist etc.

  You can see above at top left, I entered in 'sample' text,
  this is the field the user enters in, and it gets displayed real time on the right side.
  <br>
  <h2>V0.1.9 Update</h2>
  <p>
    Updated the image to reflect the current state as of V0.1.9
    Currently added the 2 dropdown boxes at bottom left.
    They each populate correctly now. Left one gets filled with the Decks,
    and right one with the Cards from the selected deck on left.
        These also fill the text boxes above with their card info correctly.
        The save/load/etc buttons do not work still,
        not started on Implementing them as of now.
  </p>
  <br><br><br>
  <h2>Small patch before V0.2.0</h2>
  <p>
  Buttons:<br>
    Home - Brings to main screen<br>
    Previous Card - Does nothing yet<br>
    Next Card - Does nothing yet<br>
    Add Card - Does nothing yet (This is one of the next ones I'm going to add)<br>
    Remove Card - Does nothing yet (This is one of the next ones I'm going to add)<br>
    Save Deck - Saves currently selected deck
    <br><br>
<img width="167" height="60" alt="Home Button" src="https://github.com/user-attachments/assets/3fc06549-f507-495a-9e51-d17e702f9e33" />
<br><br>
<img width="826" height="89" alt="Buttons 001" src="https://github.com/user-attachments/assets/708abb16-0194-4d20-bcd9-1addfdc635c5" />

  Combo Boxes:<br>
    Decks - Populates current Decks<br>
    Cards - Populates current Card within said Deck
    
<img width="449" height="60" alt="ComboBoxes" src="https://github.com/user-attachments/assets/1441887c-c71d-46ea-884b-bfe5c3413101" />

  Text Boxes:<br>
    Front + Reading + Pronunciation + Answer<br>All of these populate the respective fields of the card, showing a display to the right of what the card will look like.
      <br>- These Text Box fields can be modified, they automatically update in memory, "Save Deck" button must be used or changes will not save.
      
<img width="240" height="374" alt="TextBoxes" src="https://github.com/user-attachments/assets/c248e93b-fc94-4e5f-98ec-55970fb71ce4" />

  </p>
</p>

<h2>Card View Screen</h2>
<p>
  <img width="1919" height="1032" alt="CardScreen001" src="https://github.com/user-attachments/assets/c71b645c-1e52-420a-92c2-678fb90a92ed" />
^ This is the "CardScreen",
  The main study loop.


  Once you load a deck on prior "StudyScreen", this now shows the first card in that deck in the middle of the screen.
  You then select "how difficult this is", using the 3 colored buttons for "Hard", "Normal", and "Easy".
  Once you have chose 1 of the 3 options, that "Answer" button will be enabled and visible fully,
  rather than the lighter look it has prior.
  (For now, the logic end of things just 'functions' in this way, they do nothing yet. That's a WIP,
  think of 'Anki' SRS (Spaced Repetition System), these DifficultyValues will later be used for that purpose,
  my own method of doing an SRS.
  <br><br><br>

  <img width="1919" height="1032" alt="CardScreen002" src="https://github.com/user-attachments/assets/b84040c7-113d-4dbc-b7c4-49ad762581f1" />
^ This is another "CardScreen",
  shows the option selected, highlights the chosen one, shows the answer button/enables it.
  <br><br><br>
</p>

<h2>Button Tool Tip Example</h2>
<p>
  <img width="277" height="76" alt="ButtonToolTipExample" src="https://github.com/user-attachments/assets/e902800d-e3db-45fa-ba27-7ef663134314" />
  <br>
^ Oh and this is just a screenshot of the "ToolTip" for this "PreviousButton", which as tooltip implies,
  does nothing currently.
  <br><br><br>
</p>
