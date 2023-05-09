<script>

// Initialize firebase
import { initializeApp} from "firebase/app";
import { getFirestore, collection, onSnapshot, query, orderBy, doc, addDoc, updateDoc, deleteDoc } from "firebase/firestore";
import {firebaseConfig} from "$lib/firebaseConfig";

const firebaseApp = initializeApp(firebaseConfig);
const db = getFirestore(firebaseApp);

// Put the journal entries into the entries database
const colRef = collection(db, "entries");
let entries = [];

const unsubscribe = onSnapshot(query(colRef, orderBy("createdAt", "asc")), (querySnapshot) => {
  let fbEntries = [];
  querySnapshot.forEach((doc) => {
    let entry = {...doc.data(), id: doc.id}
    fbEntries = [entry, ...fbEntries];
  });
  entries = fbEntries;
});

// Check
console.log({firebaseApp, db});

  // Add entry
  let journal = " ";

  // Adds to database
  const addEntry = async () => {
    
    if (journal !== " ") {
      const docRef = await addDoc(collection(db, "entries"), {
        journal: journal,
        createdAt: new Date(),
      });
      journal = ""; // Clear the textarea when add is pressed

    };
  };

  async function deleteEntry(entryId) {
  await deleteDoc(doc(colRef, entryId));
}

function handleDeleteClicked(entryId) {
  if (confirm("Are you sure you want to delete this entry?")) {
    deleteEntry(entryId);
  }
}

$: console.table(entries);
</script>

<html lang="en">
    <head>
</head>

  <body>
    <header>

    </header>
    <div class="wrapper">
      <div class="content" role="main">
        <!-- TITLE OF HOMEPAGE -->
        <h1 class="title">My Journal</h1>
       </div>
       <div class="instructions">
    <br>
    This is where you can write more detailed entries for later.

    <br>

      <br> Any thoughts? <br />
      <br>
      <textarea rows="5" cols="60" placeholder = "Any thoughts?" bind:value={journal}/>
      <br>
      <br>
      <button on:click={addEntry}>Add</button>
<ul>
  {#each entries as writing}
    <li> <p> {writing.journal}  </p>     
      <p> <b> Created At: {new Date(writing.createdAt.seconds * 1000).toLocaleString()} </b> </p>
      <button on:click={() => handleDeleteClicked(writing.id)}>Delete</button>
    </li>
    
  {/each}
</ul>
</div>
</div>

</html>

