import { cp, mkdir, readdir, rm, writeFile } from 'node:fs/promises'
import { existsSync } from 'node:fs'
import path from 'node:path'
import { fileURLToPath } from 'node:url'

const frontendRoot = path.resolve(path.dirname(fileURLToPath(import.meta.url)), '..')
const distDirectory = path.join(frontendRoot, 'dist')
const hostWebDirectory = path.resolve(
  frontendRoot,
  '..',
  'Host',
  'src',
  'Host',
  'Assets',
  'Web',
)

if (!existsSync(path.join(distDirectory, 'index.html'))) {
  throw new Error('未找到 dist/index.html，请先运行 npm run build。')
}

await rm(hostWebDirectory, { recursive: true, force: true })
await mkdir(hostWebDirectory, { recursive: true })
await cp(distDirectory, hostWebDirectory, { recursive: true })

async function listFiles(directory, prefix = '') {
  const entries = await readdir(directory, { withFileTypes: true })
  const files = []

  for (const entry of entries) {
    const relativePath = prefix ? `${prefix}/${entry.name}` : entry.name
    const absolutePath = path.join(directory, entry.name)

    if (entry.isDirectory()) {
      files.push(...(await listFiles(absolutePath, relativePath)))
    } else {
      files.push(relativePath.replaceAll('\\', '/'))
    }
  }

  return files
}

const manifestName = 'avweb-host-web-manifest.json'
const files = (await listFiles(hostWebDirectory))
  .filter((file) => file !== manifestName)
  .sort()

await writeFile(
  path.join(hostWebDirectory, manifestName),
  JSON.stringify({ files }, null, 2),
  'utf8',
)

console.log(`已准备 ${files.length} 个 Host 前端资源。`)
