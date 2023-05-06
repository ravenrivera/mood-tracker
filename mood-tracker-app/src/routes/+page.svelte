<script>
  import { onMount } from "svelte";
  
  onMount(async () => {
    fetch("http://localhost:5186/api/DailyMoodPostModels")
    .then(response => response.json())
    .then(data => {
      console.log(data);
    }).catch(error => {
      console.log(error);
      return [];
    });
  });
  </script>

<html lang="en">
  <head>

    <!-- meta name="viewport" content="width=device-width, initial-scale=1" /> -->
    <!--
      Page head contains info the browser uses
    -->
    <title>Mood Tracker</title>

  </head>

  <body>

<header>
  <!-- Contains navbar -->
</header>

    <div class="wrapper">
      <div class="content" role="main">
        <!-- TITLE OF HOMEPAGE -->
        <h1 class="title">Daily Entries</h1>
       
        <div class="instructions">

          <div id="posts_container" class="entry">
        <!-- Existing entry -->
      </div>
        <div id="myDiv" class="date">
          <h2> <!-- Date --> </h2>
        </div>
          
        <!-- Add text box here, will contain the entry -->
            
              <!-- Entry is here -->

              <!-- Text break -->
              <br>
              <button>
                  Edit
                  </button>
        </div>
      </div>
    </div> 
   <footer class="footer">
   <!--    <div class="links"></div>
      
        Mood Choices
      </a> -->
     
    </footer>
    
    
  </body>
  
  
</html>

