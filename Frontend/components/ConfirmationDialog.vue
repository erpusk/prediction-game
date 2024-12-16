<template>
    <div v-if="isVisible" class="modal">
      <div class="modal-content">
        <p>{{ message }}</p>
        <button @click="confirm">Jah</button>
        <button @click="cancel">Ei</button>
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
    background: white;
    padding: 20px;
    border-radius: 5px;
  }
  </style>
  