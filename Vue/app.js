window.onload = () => {
  var app = new Vue({
    el: "#app",
    data() {
      return {
        info: null,
      };
    },
    mounted() {
      axios
        .get("https://localhost:44316/api/tasks", {
          headers: {
            "Access-Control-Allow-Origin": "https://localhost:44316",
          },
        })
        .then((res) => (this.info = res));
    },
  });
};
