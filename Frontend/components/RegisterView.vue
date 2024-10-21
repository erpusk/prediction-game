<template>
    <form name="register-form">

        <UFormGroup label="Username">
            <UInput class="border rounded-md p-2" type="text"/>
        </UFormGroup>
        
        <UFormGroup label="Email">
            <UInput class="border rounded-md p-2" type="email"/>
        </UFormGroup>
        
        <UFormGroup label="Password">
            <UInput class="border rounded-md p-2" type="password"/>
        </UFormGroup>
        
        <UFormGroup label="Confirm password">
            <UInput class="border rounded-md p-2" type="password"/>
        </UFormGroup>

        <UButton type="submit">
            Register
        </UButton>
        
    </form>
</template>