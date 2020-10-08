<template>
  <div class="side-menu-container">
    <router-link to="/">
      <img
        id="logo"
        src="../assets/images/solar_coffee_logo.png"
        alt="Solar Logo"
      />
    </router-link>
    <h1>Management Portal</h1>
    <SolarButton
      id="menuInventory"
      @button:click="goToRoute('/inventory')"
      :isFullWidth="true"
    >
      Inventory
    </SolarButton>
    <SolarButton
      id="menuCustomers"
      @button:click="goToRoute('/customers')"
      :isFullWidth="true"
    >
      Customers
    </SolarButton>
    <SolarButton
      id="menuInvoices"
      @button:click="goToRoute('/invoice/new')"
      :isFullWidth="true"
    >
      Invoices
    </SolarButton>
    <SolarButton
      id="menuOrders"
      @button:click="goToRoute('/orders')"
      :isFullWidth="true"
    >
      Orders
    </SolarButton>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import SolarButton from "@/components/SolarButton.vue";

@Component({
  name: "SideMenu",
  components: {
    SolarButton
  }
})
export default class SideMenu extends Vue {
  async goToRoute(route: string) {
    await this.$router.push(route);
  }
}
</script>

<style scoped lang="scss">
.side-menu-container {
  @import "@/scss/global.scss";

  background-color: #fcfcfc;
  height: 100vh;
  display: flex;
  flex-direction: column;
  padding: 0.8rem;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.12), 0 1px 1px rgba(0, 0, 0, 0.24);
  box-sizing: border-box;
  width: $menu-width;
}

#logo {
  width: 100%;
}

h1 {
  font-size: 1.2rem;
  margin: 1rem 0;
}
</style>
