<!-- Global layout for all pages -->

<script>
    import '../styles/global.css'
    
</script>

<header>
    <div class="navbar">
        <!-- Routing -->
        <a href="/">Home</a>
        <a href="/new-entry">New</a>
        <a href="/calendar">Calendar</a>
        <a href="/options">Options</a>
      </div>
</header>



<main>
    <!-- page content -->
    <!-- svelte will place this in slot -->
    <slot></slot>
</main>

  <style>
      /* The navigation bar */
  .navbar {
    overflow: hidden;
    background-color: #AED6F1;
    position: fixed; /* Set the navbar to fixed position */
    top: 0; /* Position the navbar at the top of the page */
    width: 100%; /* Full width */
  }
  
  /* Links inside the navbar */
  .navbar a {
    /* Text goes from right to left */
    float: right;
    display: block;
    color: #f2f2f2;
    text-align: center;
    padding: 14px 16px;
    text-decoration: none;
  }
  
  /* Change background on mouse-over */
  .navbar a:hover {
    background: #ddd;
    color: black;
  }
  </style>