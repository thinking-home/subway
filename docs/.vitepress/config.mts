import { defineConfig } from 'vitepress'

// сборки, для которых generate-api.sh кладёт справочник в docs/reference/<Сборка>/
const referenceAssemblies = [
    'ThinkingHome.DeviceModel',
    'ThinkingHome.DeviceModel.FluentApi',
    'ThinkingHome.DeviceModel.Remoting',
    'ThinkingHome.DeviceModel.Remoting.ProxyClient',
    'ThinkingHome.DeviceModel.Remoting.ProxyServer',
    'ThinkingHome.Alice',
]

export default defineConfig({
    lang: 'ru-RU',
    title: 'ThinkingHome.DeviceModel',
    description: 'Виртуальный хаб умного дома',

    // Сайт публикуется на собственном домене (docs/public/CNAME), поэтому базовый путь корневой.
    base: '/',

    themeConfig: {
        nav: [
            { text: 'Руководство', link: '/guide/what-is-it' },
            { text: 'Библиотеки', link: '/packages/' },
            { text: 'API', link: '/reference/ThinkingHome.DeviceModel/' },
        ],

        sidebar: {
            '/guide/': [
                {
                    text: 'Руководство',
                    items: [
                        { text: 'Что это', link: '/guide/what-is-it' },
                        { text: 'Быстрый старт', link: '/guide/quick-start' },
                        { text: 'Предметная область', link: '/guide/domain' },
                        { text: 'Нейтральная модель', link: '/guide/model' },
                        { text: 'Согласование с Matter', link: '/guide/matter' },
                    ],
                },
            ],
            // Библиотеки и приложения — один раздел с картой пакетов в корне.
            ...Object.fromEntries(['/packages/', '/apps/'].map(prefix => [prefix, [
                { text: 'Карта пакетов', link: '/packages/' },
                {
                    text: 'Библиотеки',
                    items: [
                        { text: 'DeviceModel — ядро', link: '/packages/device-model' },
                        { text: 'FluentApi', link: '/packages/fluent-api' },
                        { text: 'Ремоутинг: обзор', link: '/packages/remoting' },
                        { text: 'ProxyClient', link: '/packages/proxy-client' },
                        { text: 'ProxyServer', link: '/packages/proxy-server' },
                        { text: 'Alice', link: '/packages/alice' },
                    ],
                },
                {
                    text: 'Приложения',
                    items: [
                        { text: 'Proxy — облачный прокси', link: '/apps/proxy' },
                        { text: 'Home — демо-хост', link: '/apps/home' },
                        { text: 'Деплой в облако', link: '/apps/deploy' },
                        { text: 'Навык Алисы', link: '/apps/alice-skill' },
                    ],
                },
            ]])),
            '/reference/': [
                {
                    text: 'Справочник API',
                    items: referenceAssemblies.map(a => ({ text: a, link: `/reference/${a}/` })),
                },
            ],
        },

        footer: {
            message: 'Опубликовано под лицензией MIT',
            copyright: '© 2026 Dmitry Andriyanov'
        },
        
        outline: { label: 'На этой странице' },
        docFooter: { prev: 'Назад', next: 'Вперёд' },
        darkModeSwitchLabel: 'Тема',
        sidebarMenuLabel: 'Меню',
        returnToTopLabel: 'Наверх',
        search: { provider: 'local' },
        socialLinks: [{ icon: 'github', link: 'https://github.com/thinking-home/subway' }],
    },
})
