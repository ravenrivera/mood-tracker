<script>

import { initializeApp} from "firebase/app";
import { getFirestore, collection, onSnapshot, doc, addDoc, updateDoc, deleteDoc } from "firebase/firestore";
import {firebaseConfig} from "$lib/firebaseConfig";

const firebaseApp = initializeApp(firebaseConfig);
const db = getFirestore(firebaseApp);

const colRef = collection(db, "entryhistory");

console.log({firebaseApp, db});

    import { createEventDispatcher } from 'svelte';
    const dispatch = createEventDispatcher();
    
    export let show = false;
    let notes = '';
    let mood = '';
    let activity = [];
    let date = ' '
    let entryhistory = [];
    

    function handleSubmit() {
      const formData = {
        mood: mood,
        activity: activity,
        notes: notes,
        date: date
        
      };

      
      entryhistory = [formData, ...entryhistory];
      dispatch('formSubmitted', formData);
      closeForm();
    }
    
  
    function closeForm() {
      notes = '';
      mood = '';
      activity = [];
      date = '';
      show = false;
    }


    function toggleActivity(selectedActivity) {
    if (activity.includes(selectedActivity)) {
      activity = activity.filter(a => a !== selectedActivity);
    } else {
      activity = [...activity, selectedActivity];
    }
  }

  const addEntry = async () => {
    if (mood !== " ") {
      const docRef = await addDoc(collection(db, "entryhistory"), {
        mood: mood,
        activity: activity,
        notes: notes,
        createdAt: new Date()
      });

  };
};
$: console.table(entryhistory);
  </script>
  
  {#if show}
  
  <div class="popup-overlay">
    <div class="popup-container">
      <span class="close" on:click={closeForm}>&times;</span>
      <h2>New Entry</h2>
      <form on:submit|preventDefault={handleSubmit}>
        <label for="mood">Mood:</label>
        <div>
          <button type="button" on:click={() => mood = 'Great'} class:selected={mood === 'Great'} >Great</button>
          <button type="button" on:click={() => mood = 'Good'} class:selected={mood === 'Good'}>Good</button>
          <button type="button" on:click={() => mood = 'Okay'} class:selected={mood === 'Okay'}>Okay</button>
          <button type="button" on:click={() => mood = 'Bad'} class:selected={mood === 'Bad'}>Bad</button>
          <button type="button" on:click={() => mood = 'Awful'} class:selected={mood === 'Awful'}>Awful</button>
        </div>
        <label for="activity">Activity:</label>
<div>
  <button type="button" on:click={() => toggleActivity('Study')} class:selected={activity.includes('Study')} >Study</button>
  <button type="button" on:click={() => toggleActivity('Exercise')} class:selected={activity.includes('Exercise')}>Exercise</button>
  <button type="button" on:click={() => toggleActivity('Go Out')} class:selected={activity.includes('Go Out')}>Go Out</button>
  <button type="button" on:click={() => toggleActivity('Reading')} class:selected={activity.includes('Reading')}>Reading</button>
  <button type="button" on:click={() => toggleActivity('Movie/TV/Game')} class:selected={activity.includes('Movie/TV/Game')}>Movie/TV/Game</button>
  <button type="button" on:click={() => toggleActivity('Hobby')} class:selected={activity.includes('Hobby')}>Hobby</button>
</div>
        <label for="notes">Notes:</label>
        <textarea id="notes" bind:value={notes} required></textarea>
       <!--

       --> <button type="submit" disabled={!mood || !date} on:click={addEntry}>Add</button>

    </form>
      {#if !mood}
  <p>Please select a mood.</p>
{/if}
    </div>
  </div>
  
  {/if}
  
  <div style="text-align: left;">
    {#if entryhistory.length > 0}
      <h3>Previous Entries:</h3>
   <ul>
        {#each entryhistory as entry}
          <li>
            <p>
                {entry.mood}<br>
                {entry.activity}<br>
                {entry.notes}<br>
                {entry.date}
              </p>
            </li>
        {/each}
      </ul>
    {/if}
  </div>
  
  <style>
    /* Popup styles */
    .popup-overlay {
      position: fixed;
      z-index: 1;
      left: 0;
      top: 0;
      width: 100%;
      height: 100%;
      background-color: rgba(0, 0, 0, 0.4);
      display: flex;
      align-items: center;
      justify-content: center;
    }
  
    .popup-container {
      background-color: white;
      padding: 2rem;
      border-radius: 8px;
      position: relative;
      width: 400px;
      max-width: 90%;
      max-height: 90%;
      overflow: auto;
      box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.3);
    }
  
    .popup-container .close {
      position: absolute;
      top: 0;
      right: 0;
      padding: 1rem;
      cursor: pointer;
    }
  
    /* Text box styles */
    #notes {
      width: 100%;
      height: 10rem;
      border: none;
      border-radius: 4px;
      padding: 0.5rem;
      margin-top: 1rem;
      box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.3);
      font-size: 1rem;
      line-height: 1.5rem;
      resize: vertical;
    }

    /* Selected button style */
    .selected {
      background-color: #4a5568;
    }
  
  </style>
  