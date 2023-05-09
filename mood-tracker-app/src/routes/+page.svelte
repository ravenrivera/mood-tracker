<script>
import { initializeApp} from "firebase/app";
import { getFirestore, collection, onSnapshot, query, orderBy, doc, addDoc, updateDoc, deleteDoc } from "firebase/firestore";
import {firebaseConfig} from "$lib/firebaseConfig";
import PopupForm from './PopupForm.svelte';

const firebaseApp = initializeApp(firebaseConfig);
const db = getFirestore(firebaseApp);
const colRef = collection(db, "entryhistory");

let notesList = [];
let moodCounts = {
  "Great": 0,
  "Good" : 0,
  "Okay": 0,
  "Bad" : 0,
  "Awful": 0
}

const unsubscribe = onSnapshot(query(colRef, orderBy("createdAt", "asc")), (querySnapshot) => {
  let fbEntries = [];
  querySnapshot.forEach((doc) => {
    let entry = {...doc.data(), id: doc.id}
    fbEntries = [entry, ...fbEntries];
    moodCounts[entry.mood] = (moodCounts[entry.mood] || 0) + 1;
  });
  console.table(fbEntries);
  notesList = fbEntries;
});

async function deleteEntry(entryId) {
  await deleteDoc(doc(colRef, entryId));
}

function handleDeleteClicked(entryId) {
  if (confirm("Are you sure you want to delete this entry?")) {
    deleteEntry(entryId);
  }
}

  function handleFormSubmitted(event) {
    notesList = [...notesList, event.detail.notes];
    moodCounts[event.detail.notes.mood] = (moodCounts[event.detail.notes.mood] || 0) + 1;
  }
</script>


<html lang="en">
  <head>

    <title>Mood Tracker</title>

  </head>

  <body>

<header>
  <!-- Contains navbar -->
</header>

    <div class="wrapper">
      <div class="content" role="main">
        <!-- TITLE OF HOMEPAGE -->
        <h1 class="title">Mood Tracker</h1>
       
        <div class="instructions">
              <br>
              <button on:click={() => PopupForm.show = true}>New Entry</button>
<PopupForm on:formSubmitted={handleFormSubmitted} show={PopupForm.show}/>
{#if notesList.length > 0}
  <h2>Moods This Week:</h2>
  <ul>
    {#each Object.entries(moodCounts) as [mood, count]}
      <li>{mood}: {count}</li>
    {/each}
  </ul>
<h2>Previous Entries</h2>
  <!-- display notes list here -->
{:else}
  <p>No notes added yet.</p>
{/if}

<ul>
  {#each notesList as writing}
    <li>
      <p> Mood: {writing.mood} </p>
      <p> Activity: {writing.activity} </p>
      <p> Note: {writing.notes} </p>
      <p> <b> Created At: {new Date(writing.createdAt.seconds * 1000).toLocaleString()} </b> </p>
      <button on:click={() => handleDeleteClicked(writing.id)}>Delete</button>
    </li>
  {/each}
</ul>
        </div>
      </div>
    </div> 
   <footer class="footer">

    </footer>
    
  </body>
  
</html>

