<template>
    <form name="login-form">
        <UFormGroup label="Username">
            <UInput class="border rounded-md p-2" type="text"/>
        </UFormGroup>

        <UFormGroup label="Password">
            <UInput class="border rounded-md p-2" type="password"/>
        </UFormGroup>
        
        <UButton type="submit">
            Login
        </UButton>
    </form>
</template>