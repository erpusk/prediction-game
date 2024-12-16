<template>
    <div v-if="isVisible" class="modal">
      <div class="modal-content">
        <p>{{ message }}</p>
        <div class="button-container">
          <button @click="confirm" class="yes-button">Yes</button>
          <button @click="cancel" class="no-button">No</button>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    props: {
      isVisible: Boolean,
      message: String
    },
    methods: {
      confirm() {
        this.$emit('confirmed');
      },
      cancel() {
        this.$emit('cancelled');
      }
    }
  }
  </script>
  
  <style>
.modal {
  position: fixed;
  z-index: 1000;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal-content {
  background: rgb(215, 211, 211);
  padding: 20px;
  border-radius: 5px;
  text-align: center;
  color: black;
}

.button-container {
  display: flex;
  justify-content: space-between;
  margin-top: 20px;
}

.yes-button {
  background-color: #FF4136; /* Red color, change to any color you prefer */
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s; /* Optional: for a smooth color transition */
}
.yes-button:hover {
  background-color: #E82C0C; /* Darker shade of red for hover effect */
}

.no-button {
  background-color: #2ECC40; /* Green color, change to any color you prefer */
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s; /* Optional: for a smooth color transition */
}

.no-button:hover {
  background-color: #29B635; /* Darker shade of green for hover effect */
}
</style>
Kuidas nup
  