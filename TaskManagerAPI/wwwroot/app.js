window.onload = () => {
  var app = new Vue({
    el: "#app",
    data: {
      todos: [],
    },
    mounted() {
      axios
        .get("https://localhost:44316/api/tasks")
        .then((res) => (this.todos = res.data.items));
    },
  });

  Vue.Component("tree-view-list", {
    template: "#task-template",
    props: {
      task: Object,
    },
    data: function () {
      return {
        isOpen: false,
      };
    },
    computed: {
      isFolder: function () {
        return this.task.subtasks && this.task.subtasks.lenght;
      },
    },
    methods: {
      toggle: function () {
        if (this.isFolder) {
          this.isOpen = !this.isOpen;
        }
      },
    },
  });
};
