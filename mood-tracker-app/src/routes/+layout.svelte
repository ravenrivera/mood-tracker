<!-- Global layout for all pages -->

<script>
    import '../styles/global.css'
    
</script>

<header>
    <div class="navbar">
        <!-- Routing -->
        <a href="/">Home</a>
        <a href="/myjournal">My Journal</a>
      </div>
</header>

<main>
    <!-- page content -->
    <!-- svelte will place this in slot -->
    <slot></slot>
</main>

<!--Put navbar here if there's a problem-->